using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using FileDataAccess;
using FileDataAccess.Database;

namespace LocalSearchEngine.FileManager
{
    /// <summary>
    /// The file agent is responsible for managing the indexing process,
    /// Keep track of the changes at runtime level in the structure of files within the local search active directory 
    /// including subdirectories inside.
    /// </summary>
    public class FileAgent
    {
        #region Extension Types
        /// <summary>
        /// Defines the supported extension images
        /// </summary>
        public static string[] ExtensionImagesFile = { ".bmp", ".jpg", ".png", ".jpeg", ".tif", ".tiff", ".jif", ".jfif", ".jp2", ".jpx", ".j2k", ".j2c" };
        /// <summary>
        /// Defines the supported extension documents that may contain images within
        /// </summary>
        public static string[] ExtensionImagesContentFile = { ".docx", ".odt", ".pptx" };
        /// <summary>
        /// Defines the supported extension for pure text documents 
        /// </summary>
        public static string[] ExtensionOnlyTextFile = { ".txt", ".rtf", ".htm" };
        /// <summary>
        /// Temporary folder where document embedded images are temporarily saved
        /// </summary>
        public static string TempFolder = Path.Combine(Path.GetTempPath(), "ImageSearchingTempFiles");
        /// <summary>
        /// Get all supported extension files
        /// </summary>
        public static string[] GetSupportedFiles
        {
            get
            {
                var z = new string[ExtensionImagesFile.Length + ExtensionImagesContentFile.Length];
                ExtensionImagesFile.CopyTo(z, 0);
                ExtensionImagesContentFile.CopyTo(z, ExtensionImagesFile.Length);
                return z;
            }
        }
        /// <summary>
        /// Get all supported extension files [Appending *. Regex for filter methods] 
        /// </summary>
        public static string[] GetSupportedFilesFilter
        {
            get
            {
                var z = new string[ExtensionImagesFile.Length + ExtensionImagesContentFile.Length];
                ExtensionImagesFile.Select(e => "*" + e).ToArray().CopyTo(z, 0);
                ExtensionImagesContentFile.Select(e => "*" + e).ToArray().CopyTo(z, ExtensionImagesFile.Length);
                return z;
            }
        }

        #endregion

        #region Variables
        /// <summary>
        /// Working Directory
        /// </summary>
        private string _directory;

        /// <summary>
        /// Page size to get list of all indexed files in batches
        /// </summary>
        private const int PageDataBaseSize = 10000;
        /// <summary>
        /// File Changes Watcher 
        /// </summary>
        public Task WatchFileSystemTask;
        #endregion

        /// <summary>
        /// File Agent Instace with the working directory
        /// </summary>
        /// <param name="initialDirectory">Directory target</param>
        public FileAgent(string initialDirectory)
        {
            this._directory = initialDirectory;
            //Create Temp Folder
            Directory.CreateDirectory(TempFolder);
        }

        public FileAgent()
        {
            //Create Temp Folder
            Directory.CreateDirectory(TempFolder);
        }

        public void SetInitialDirectory(string initialDirectory)
        {
            this._directory = initialDirectory;
        }
        /// <summary>
        /// Starts the indexation of the working directory
        /// Adds new files to the DB and extract images from Documents 
        /// </summary>
        public void InitializeIndexation()
        {
            //Process Indexation
            ProcessIndexation(this._directory, GetSupportedFilesFilter, IndexFile);
            WatchFileSystemTask = Task.Factory.StartNew(() => RunWatcher(_directory)
                , TaskCreationOptions.LongRunning);
        }
        /// <summary>
        /// Starts the update indexation database records
        /// * Deletes the missing files
        /// * Add new files [not indexed]
        /// [Warning] This method does not support changes of files yet.
        /// </summary>
        public void UpdateIndexation()
        {
            UpdateDataBaseIndexation();
            ProcessIndexation(this._directory, GetSupportedFilesFilter, TryIndexFile);
            WatchFileSystemTask = Task.Factory.StartNew(() => RunWatcher(_directory)
                , TaskCreationOptions.LongRunning);
        }

        #region File Watcher Control
        /// <summary>
        /// Starts new task [Thread] to watch the directory environment
        /// Keeps track of Modifications, Deletions, Additions of new supported files
        /// </summary>
        /// <param name="directory">Environment Directory</param>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void RunWatcher(string directory)
        {
            var fileSystemWatcher = new FileSystemWatcher
            {
                Filter = "*.*",
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Path = directory,
                IncludeSubdirectories = true
            };
            // Add event handlers.
            //fileSystemWatcher.Changed += OnFileChanged;
            fileSystemWatcher.Created += OnFileCreated;
            fileSystemWatcher.Deleted += OnFileDeleted;
            fileSystemWatcher.Renamed += OnFileRenamed;

            // Begin watching.
            fileSystemWatcher.EnableRaisingEvents = true;

            while (true) ;
        }
        /// <summary>
        /// Action taked on File Renamed Event
        /// </summary>
        /// <param name="sender">Watcher Object</param>
        /// <param name="e">Event Information Details</param>
        private static void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            if ((Path.GetDirectoryName(e.FullPath) == TempFolder)) return;

            if (GetSupportedFiles.Any(f => f == Path.GetExtension(e.Name)))
            {
                var file = new FileInfo(e.FullPath);
                var document = DataBaseManager.GetFileByFullName(e.OldFullPath);
                if (document != null)
                {
                    document.Name = file.Name;
                    document.FolderPath = file.DirectoryName;

                    DataBaseManager.UpdateFile(document);
                }
                else
                {
                    IndexFile(file);
                }
                Console.WriteLine("{0:dd/MM/yy H:mm:ss} [Rename Event] File Record Index Updated {1}", DateTime.Now, file.FullName);
            }

        }
        /// <summary>
        /// Action taked on File Deleted Event
        /// </summary>
        /// <param name="sender">Watcher Object</param>
        /// <param name="e">Event Information Details</param>
        private static void OnFileDeleted(object sender, FileSystemEventArgs e)
        {
            if ((Path.GetDirectoryName(e.FullPath) == TempFolder)) return;

            if (GetSupportedFiles.Any(f => f == Path.GetExtension(e.Name)))
            {
                var document = DataBaseManager.GetFileByFullName(e.FullPath);
                if (document == null) return;

                foreach (var image in DataBaseManager.GetFileInternalImages(document))
                {
                    DeleteTemporalImage(image.TempKeyName);
                }

                DataBaseManager.DeleteDocumentFile(document);

                Console.WriteLine("{0:dd/MM/yy H:mm:ss} [Delete Event] File Record Index Updated {1}", DateTime.Now, e.FullPath);
            }

        }
        /// <summary>
        /// Action taked on File Created Event
        /// </summary>
        /// <param name="sender">Watcher Object</param>
        /// <param name="e">Event Information Details</param>
        private static void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            if ((Path.GetDirectoryName(e.FullPath) == TempFolder)) return;

            if (GetSupportedFiles.Any(f => f == Path.GetExtension(e.Name)))
            {
                var file = new FileInfo(e.FullPath);
                var document = DataBaseManager.GetFileByFullName(e.FullPath);
                if (document != null) return;
                IndexFile(file);
                Console.WriteLine("{0:dd/MM/yy H:mm:ss} [Create Event] File Record Index Updated {1}", DateTime.Now, e.FullPath);
            }

        }
        /// <summary>
        /// Action taked on File Changed Event
        /// </summary>
        /// <param name="sender">Watcher Object</param>
        /// <param name="e">Event Information Details</param>
        private static void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            if ((Path.GetDirectoryName(e.FullPath) == TempFolder)) return;
            
            if (GetSupportedFiles.Any(f => f == Path.GetExtension(e.Name)))
            {
                var document = DataBaseManager.GetFileByFullName(e.FullPath);
                if (document != null)
                {
                    DataBaseManager.DeleteDocumentFile(document);
                }
                IndexFile(new FileInfo(e.FullPath));
                Console.WriteLine("{0:dd/MM/yy H:mm:ss} [Changed Event] File Record Index Updated {1}", DateTime.Now, e.FullPath);
            }
        }

        #endregion

        #region Index Management
        /// <summary>
        /// Function that starts exploring the files inside working directory 
        /// * Non Recursive File Retrieve Function
        /// * Access Denied Exception Support
        /// </summary>
        /// <param name="path">Base Directory</param>
        /// <param name="filePattern">Supported Indexation Files [Extension files to be indexed]</param>
        /// <param name="indexationFunction">Indexation Action Function</param>
        private static void ProcessIndexation(string path, string[] filePattern, Action<FileInfo> indexationFunction)
        {
            var queue = new Queue<string>();
            queue.Enqueue(path);

            while (queue.Count > 0)
            {
                path = queue.Dequeue();
                //Discard Temp Folder
                if (path == TempFolder) continue;

                try
                {
                    foreach (var subDir in System.IO.Directory.GetDirectories(path))
                    {
                        queue.Enqueue(subDir);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }

                try
                {
                    //Process Files
                    foreach (var fileType in filePattern)
                    {
                        //Execute Function for Indexation
                        System.IO.Directory.GetFiles(path, fileType).AsParallel().ForAll(e => indexationFunction.Invoke(new FileInfo(e)));
                    }

                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
            }
        }
        /// <summary>
        /// Index file record in database and attach related document images
        /// </summary>
        /// <param name="info">File System information</param>
        private static void IndexFile(FileInfo info)
        {
            //is Valid Image File 
            //Check if the size of the image is allowed to be indexed, other restrictions can be added in [Restrict] Label.
            bool validImage = false;

            var file = new DocumentFile()
            {
                Id = Guid.NewGuid(),
                CreatedDate = info.CreationTime,
                ModifiedDate = info.LastWriteTime,
                FolderPath = info.DirectoryName,
                Name = info.Name,
                ItemType = info.Extension,
                Size = info.Length
            };

            //Check if the file supports embedded image files
            if (ExtensionImagesContentFile.Any(e => e.Equals(Path.GetExtension(file.Name))))
            {
                //Try search inside document images
                var results = ImageFileExtractor.ExtractImagesFromFile(GetDocumentFullPathName(file), TempFolder, ExtensionImagesFile);
                //Append all related images
                results.ForEach(e => file.DocumentImages.Add(e));

                validImage = true;
            }
            else
            {
                //The file itself is an image
                validImage = AttachImageFile(file);
            }

            if (!validImage) return;
            //Save record on DB
            DataBaseManager.SaveFile(file);
            Console.WriteLine("Indexed : {0}", info.Name);
        }
        /// <summary>
        /// Function used in Update Database records 
        /// This function verify if the file is already indexed
        /// </summary>
        /// <param name="info">File Information</param>
        private static void TryIndexFile(FileInfo info)
        {
            if (DataBaseManager.IsFileAlreadyIndexed(info))
                return;
            IndexFile(info);
        }

        /// <summary>
        /// Only Checks if files still exists if no the missing db records are removed
        /// </summary>
        private static void UpdateDataBaseIndexation()
        {
            //TEST FOR FILES FROM DATABASE
            var totalIndexed = DataBaseManager.GetTotalFilesIndexed();

            var pagesCount = (totalIndexed / PageDataBaseSize);

            if (pagesCount * PageDataBaseSize < totalIndexed) pagesCount++;

            for (var i = 0; i < pagesCount; i++)
            {
                //Get indexed Result
                var results = DataBaseManager.GetIndexedFilesPaged(i, PageDataBaseSize);

                //Search if file exists
                foreach (var file in results.Where(file => !File.Exists(GetDocumentFullPathName(file))))
                {
                    //Delete Temporal Images if file not found
                    foreach (var image in DataBaseManager.GetFileInternalImages(file))
                    {
                        DeleteTemporalImage(image.TempKeyName);
                    }
                    //Delete Image In DB 
                    DataBaseManager.DeleteDocumentFile(file);
                }
            }
        }
        /// <summary>
        /// Associate Any image with the related file.
        /// Two managed cases are :
        /// 1) File is an image itself
        /// 2) Image File is contained in a document file
        /// </summary>
        /// <param name="file">Realted File</param>
        /// <returns>Resulting Operation Process</returns>
        private static bool AttachImageFile(DocumentFile file)
        {
            try
            {
                using (var fs = new FileStream(GetDocumentFullPathName(file), FileMode.Open, FileAccess.Read))
                {
                    using (var img = Image.FromStream(fs, true, false))
                    {
                        //Image Restriction Code [Restrict]
                        if (img.Width <= 320 || img.Height <= 240) { return false; }

                        var docImg = new DocumentImage()
                        {
                            Id = Guid.NewGuid(),
                            PixelFormat = img.PixelFormat.ToString(),
                            Width = img.Width,
                            Height = img.Height,
                            IsWithinFile = false
                        };
                        //Attach image to file
                        file.DocumentImages.Add(docImg);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("[External] File Not supported {0}:{1}", GetDocumentFullPathName(file), e.Message);
                return false;
            }
        }

        #endregion

        #region I/O File Management
        
        private static List<string> GetFilesFromDirectory(string path, string[] filePattern)
        {
            var queue = new Queue<string>();
            var files = new List<string>();
            queue.Enqueue(path);

            while (queue.Count > 0)
            {
                path = queue.Dequeue();
                try
                {
                    foreach (var subDir in System.IO.Directory.GetDirectories(path))
                    {
                        queue.Enqueue(subDir);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }

                try
                {
                    foreach (var fileType in filePattern)
                    {
                        //Added "*" For Filter *.<fileExtension>
                        files.AddRange(System.IO.Directory.GetFiles(path, "*" + fileType));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
            }
            return files;
        }
        /// <summary>
        /// Gets the full path of a document
        /// </summary>
        /// <param name="file">Document file</param>
        /// <returns>Full file path</returns>
        public static string GetDocumentFullPathName(DocumentFile file)
        {
            return Path.Combine(file.FolderPath, file.Name);
        }
        /// <summary>
        /// Delete temporal image file from the TMP System Folder
        /// </summary>
        /// <param name="file">File to delete</param>
        private static void DeleteTemporalImage(string file)
        {
            try
            {
                File.Delete(Path.Combine(TempFolder, file));
            }
            catch (Exception e)
            {
                //TODO LOG INFO
                Console.WriteLine(e.Message);
            }
        }

        #endregion

    }
}
