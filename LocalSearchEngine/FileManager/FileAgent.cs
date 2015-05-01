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
        public static string[] SupportedDocuments =
        {
            "*.bmp", "*.gif", "*.jpg", "*.png", "*.jpeg", "*.tif", "*.tiff", "*.jif", "*.jfif", "*.jp2", "*.jpx", "*.j2k", "*.j2c" ,
            "*.doc", "*.docx", "*.txt", "*.rtf", "*.odt"
        };
        public static string[] DocTypes = { ".doc", ".docx", ".txt", ".rtf", ".odt" };

        public string Directory;

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public FileAgent(string initialDirectory)
        {
            this.Directory = initialDirectory;
        }

        public void InitializeIndexation()
        {
            var fileList = GetFilesFromDirectory(this.Directory, SupportedDocuments);

            //fileList.ForEach(e => IndexFile(new FileInfo(e)));
            fileList.AsParallel().ForAll(e => IndexFile(new FileInfo(e)));
        }

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
                        files.AddRange(System.IO.Directory.GetFiles(path, fileType));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
            }
            return files;
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


            if (DocTypes.Contains(file.ItemType))
            {
                //Search Inside Document


            }
            else
            {
                //File itself is an Image
                CreateImageFromFile(file);
            }

            DataBaseManager.SaveFile(file);
        }

        private static void CreateImageFromFile(DocumentFile file)
        {
            try
            {
                var img = Image.FromFile(GetFullName(file));

                var docImg = new DocumentImage()
                {
                    Id = Guid.NewGuid(),
                    PixelFormat = img.PixelFormat.ToString(),
                    Width = img.Width,
                    Height = img.Height,
                };
                file.Images.Add(docImg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + file.Name);
            }
        }

        public static string GetFullName(DocumentFile file)
        {
            return Path.Combine(file.FolderPath, file.Name);
        }

        #endregion
    }
}
