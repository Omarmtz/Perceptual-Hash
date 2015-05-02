using System;
using System.Collections;
using System.Collections.Generic;
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

        public static string[] ExtensionImagesFile = { ".bmp", ".gif", ".jpg", ".png", ".jpeg", ".tif", ".tiff", ".jif", ".jfif", ".jp2", ".jpx", ".j2k", ".j2c" };

        public static string[] ExtensionImagesContentFile = { ".docx", ".odt", ".pptx" };

        public static string[] ExtensionOnlyTextFile = { ".txt", ".rtf", ".htm" };

        public static string TempFolder = Path.Combine(Path.GetTempPath(),"ImageSearchingTempFiles");
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
            var fileList = GetFilesFromDirectory(this.Directory, GetSupportedFiles);
            //CHECK FOR PARALLEL PROCESSING
            fileList.AsParallel().ForAll(e => IndexFile(new FileInfo(e)));
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
                var img = Image.FromFile(GetDocumentFullPathName(file));

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
            catch (OutOfMemoryException outmemException)
            {
                Console.WriteLine("File Not supported {0}:{1}", file.Name, outmemException.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + file.Name);
            }
        }

        public static string GetDocumentFullPathName(DocumentFile file)
        {
            return Path.Combine(file.FolderPath, file.Name);
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

        #endregion
    }
}
