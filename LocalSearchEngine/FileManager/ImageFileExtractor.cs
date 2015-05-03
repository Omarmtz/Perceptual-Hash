using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocalSearchEngine.DataAccess;

namespace LocalSearchEngine.FileManager
{
    public class ImageFileExtractor
    {
        public static List<DocumentImage> ExtractImagesFromFile(string path, string destinationDirectory, string[] extensions)
        {
            var results = new List<DocumentImage>();
            try
            {
                using (var archive = ZipFile.OpenRead(path))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (extensions.Any(e => e.Equals(Path.GetExtension(entry.Name))))
                        {
                            var attachmentName = Guid.NewGuid() + entry.Name;
                            var fileAttachmentPath = Path.Combine(destinationDirectory, attachmentName);
                            entry.ExtractToFile(fileAttachmentPath);

                            results.Add(GetImageInformation(fileAttachmentPath));
                            Console.WriteLine("Extracting {0} to {1} ", attachmentName, destinationDirectory);
                        }
                    }
                }
                return results;
            }
            catch (Exception e)
            {
                //TODO EXCEPTION LOGIN
                Console.WriteLine(e.Message + path);
                return results;
            }
        }

        private static DocumentImage GetImageInformation(string filePath)
        {
            try
            {
               using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var img = Image.FromStream(fs, true, false))
                    {
                        var docImg = new DocumentImage()
                        {
                            Id = Guid.NewGuid(),
                            PixelFormat = img.PixelFormat.ToString(),
                            Width = img.Width,
                            Height = img.Height,
                            IsWithinFile = true,
                            TempKeyName = Path.GetFileName(filePath)

                        };
                        return docImg;
                    }
                }
            }
            catch (Exception e)
            {
                //TODO EXCEPTION LOGIN
                Console.WriteLine("[Internal] File Not supported {0}:{1}", filePath, e.Message);
            }
            return null;
        }

    }
}
