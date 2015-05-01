using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace LocalSearchEngine.FileManager
{
    public class FileAgent
    {
        public static string[] ImageTypes = { "*.bmp", "*.gif", "*.jpg", "*.png", "*.jpeg", "*.tif", "*.tiff", "*.jif", "*.jfif", "*.jp2", "*.jpx", "*.j2k", "*.j2c" };
        public static string[] DocTypes = { "*.doc","*.docx","*.txt","*.rtf","*.odt"};

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public FileAgent(string InitialDirectory)
        {

        }

        #region I/O File Management
        private static IList GetFilesFromDirectory(string path, string[] filePattern)
        {
            var queue = new Queue<string>();
            var files = new List<string>();
            queue.Enqueue(path);

            while (queue.Count > 0)
            {
                path = queue.Dequeue();
                try
                {
                    foreach (var subDir in Directory.GetDirectories(path))
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
                        files.AddRange(Directory.GetFiles(path, fileType));
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
