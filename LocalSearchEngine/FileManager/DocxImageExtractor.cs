using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalSearchEngine.FileManager
{
    class DocxImageExtractor
    {
        //TODO ARQUITECTURE LOGIC
        public static void ExtractImages(string path)
        {
             using (ZipArchive archive = ZipFile.OpenRead(path))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    Console.WriteLine("NAME" + entry.Name);
                    if (entry.FullName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                    {
                        
                        entry.ExtractToFile(Path.Combine(Path.Combine(@"E:\Users\Desktop\Results"), entry.FullName),true);
                        Console.WriteLine("Extracting to "+ Path.GetTempPath());
                    }
                }
            } 
        }

    }
}
