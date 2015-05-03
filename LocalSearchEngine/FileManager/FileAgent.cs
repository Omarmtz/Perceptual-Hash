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
        public string[] GetSupportedFiles
        {
            get
            {
                var z = new string[ExtensionImagesFile.Length + ExtensionImagesContentFile.Length];
                ExtensionImagesFile.CopyTo(z, 0);
                ExtensionImagesContentFile.CopyTo(z, ExtensionImagesFile.Length);
                return z;
            }
        }

        public string[] GetSupportedFilesFilter
        {
            get
            {
                var z = new string[ExtensionImagesFile.Length + ExtensionImagesContentFile.Length];
                ExtensionImagesFile.Select(e => "*" + e).ToArray().CopyTo(z, 0);
                ExtensionImagesContentFile.Select(e => "*" + e).ToArray().CopyTo(z, ExtensionImagesFile.Length);
                return z;
            }
        }

        //Page size to get list of all indexed files in batches
        private static int PageDataBaseSize = 1000;

        #endregion

        #region Variables
        private string Directory;

        #endregion

        public FileAgent(string initialDirectory)
        {
            this.Directory = initialDirectory;
            //Create Temp Folder
            System.IO.Directory.CreateDirectory(TempFolder);
        }

        public void InitializeIndexation()
        {
            //Process Indexation
            ProcessIndexation(this.Directory, GetSupportedFilesFilter, IndexFile);
        }

        private static void ProcessIndexation(string path, string[] filePattern, Action<FileInfo> indexationFunction)
        {
            var queue = new Queue<string>();
            queue.Enqueue(path);

            while (queue.Count > 0)
            {
                path = queue.Dequeue();
                try
                {
                    foreach (var subDir in System.IO.Directory.GetDirectories(path))
                    {
                        //Discard TempFolder
                        if (subDir != TempFolder)
                        {
                            queue.Enqueue(subDir);
                        }
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

        public void CheckForUpdates()
        {
            var fileList = GetFilesFromDirectory(this.Directory, GetSupportedFiles);

            var totalIndexed = DataBaseManager.GetTotalFilesIndexed();

            var pagesCount = (totalIndexed / PageDataBaseSize);

            if (pagesCount * PageDataBaseSize < totalIndexed) pagesCount++;

            for (int i = 0; i < pagesCount; i++)
            {
                var results = DataBaseManager.GetIndexedFilesPaged(i, PageDataBaseSize);
            }
        }

        #region File Management

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

        #endregion
    }
}
