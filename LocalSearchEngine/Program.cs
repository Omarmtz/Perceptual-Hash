using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocalSearchEngine.FileManager;

namespace LocalSearchEngine
{
    class Program
    {
        static void Main()
        {
            var patterns = new string[] {"*.*"};
            var files = GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), patterns);
            foreach (string file in files)
            {
                Console.WriteLine(file);
            }
            Console.WriteLine("Files found {0}",files.Count);

            Console.ReadLine();
        }

        
    }
}
