using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileDataAccess.Database;


namespace LocalSearchEngine.FileManager
{
    /// <summary>
    /// Class that extracts DOCX, ODT , PPTX 
    /// Zip Extraction of internal files, without using internal definition structure Xml files
    /// </summary>
    public class ImageFileExtractor
    {
        /// <summary>
        /// This methods tries to extract images from internal files documents 
        /// </summary>
        /// <param name="path">Document file path</param>
        /// <param name="destinationDirectory">Temporal directory to save all extracted images </param>
        /// <param name="extensions">Definition of supported image extensions</param>
        /// <returns>List of document images</returns>
        public static List<DocumentImage> ExtractImagesFromFile(string path, string destinationDirectory, string[] extensions)
        {
            var results = new List<DocumentImage>();
            try
            {
                //open as Zip
                using (var archive = ZipFile.OpenRead(path))
                {
                    //Check every entry to extract supported extension patterns of images
                    foreach (var entry in archive.Entries)
                    {
                        if (!extensions.Any(e => e.Equals(Path.GetExtension(entry.Name)))) continue;
                        
                        var attachmentName = Guid.NewGuid() + entry.Name;
                        var fileAttachmentPath = Path.Combine(destinationDirectory, attachmentName);
                        entry.ExtractToFile(fileAttachmentPath);
                        results.Add(GetImageInformation(fileAttachmentPath));
                        
                        Console.WriteLine("Extracting {0} to {1} ", attachmentName, destinationDirectory);
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
        /// <summary>
        /// Get a quick Image information from image file
        /// </summary>
        /// <param name="filePath">File Path</param>
        /// <returns>Document Image</returns>
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
