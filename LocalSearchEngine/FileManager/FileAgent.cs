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
using LocalSearchEngine.DataAccess;

namespace LocalSearchEngine.FileManager
{
    public class FileAgent
    {
        #region Extension Types

        public static string[] ExtensionImagesFile = { ".bmp", ".jpg", ".png", ".jpeg", ".tif", ".tiff", ".jif", ".jfif", ".jp2", ".jpx", ".j2k", ".j2c" };

        public static string[] ExtensionImagesContentFile = { ".docx", ".odt", ".pptx" };

        public static string[] ExtensionOnlyTextFile = { ".txt", ".rtf", ".htm" };

        public static string TempFolder = Path.Combine(Path.GetTempPath(), "ImageSearchingTempFiles");
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
        private readonly string _directory;

        //Page size to get list of all indexed files in batches
        private const int PageDataBaseSize = 1000;

        public Task WatchFileSystemTask;
        #endregion

        public FileAgent(string initialDirectory)
        {
            this._directory = initialDirectory;
            //Create Temp Folder
            Directory.CreateDirectory(TempFolder);

            WatchFileSystemTask = Task.Factory.StartNew(() => RunWatcher(_directory)
                , TaskCreationOptions.LongRunning);

        }

        public void InitializeIndexation()
        {
            //Process Indexation
            ProcessIndexation(this._directory, GetSupportedFilesFilter, IndexFile);
        }

        public void UpdateIndexation()
        {
            UpdateDataBaseIndexation();
            ProcessIndexation(this._directory, GetSupportedFilesFilter, TryIndexFile);
        }

        #region File Watcher Control

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

        private static void OnFileRenamed(object sender, RenamedEventArgs e)
        {
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

        private static void OnFileDeleted(object sender, FileSystemEventArgs e)
        {
            if (GetSupportedFiles.Any(f => f == Path.GetExtension(e.Name)))
            {
                var document = DataBaseManager.GetFileByFullName(e.FullPath);
                if (document == null) return;
                DataBaseManager.DeleteDocumentFile(document);
                Console.WriteLine("{0:dd/MM/yy H:mm:ss} [Delete Event] File Record Index Updated {1}", DateTime.Now, e.FullPath);
            }

        }

        private static void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            if (GetSupportedFiles.Any(f => f == Path.GetExtension(e.Name)))
            {
                var file = new FileInfo(e.FullPath);
                var document = DataBaseManager.GetFileByFullName(e.FullPath);
                if (document != null) return;
                IndexFile(file);
                Console.WriteLine("{0:dd/MM/yy H:mm:ss} [Create Event] File Record Index Updated {1}", DateTime.Now, e.FullPath);
            }

        }

        private static void OnFileChanged(object sender, FileSystemEventArgs e)
        {
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

        private static void IndexFile(FileInfo info)
        {
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


            if (ExtensionImagesContentFile.Any(e => e.Equals(Path.GetExtension(file.Name))))
            {
                //Search Inside Document
                var results = ImageFileExtractor.ExtractImagesFromFile(GetDocumentFullPathName(file), TempFolder, ExtensionImagesFile);
                results.ForEach(e => file.Images.Add(e));
            }
            else
            {
                //File itself is an Image
                AttachImageFile(file);
            }

            DataBaseManager.SaveFile(file);
            Console.WriteLine("Indexed : {0}", info.Name);
        }

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

        private static void AttachImageFile(DocumentFile file)
        {
            try
            {
                using (var fs = new FileStream(GetDocumentFullPathName(file), FileMode.Open, FileAccess.Read))
                {
                    using (var img = Image.FromStream(fs, true, false))
                    {
                        var docImg = new DocumentImage()
                        {
                            Id = Guid.NewGuid(),
                            PixelFormat = img.PixelFormat.ToString(),
                            Width = img.Width,
                            Height = img.Height,
                            IsWithinFile = false
                        };
                        file.Images.Add(docImg);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[External] File Not supported {0}:{1}", GetDocumentFullPathName(file), e.Message);
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

        public static string GetDocumentFullPathName(DocumentFile file)
        {
            return Path.Combine(file.FolderPath, file.Name);
        }

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
