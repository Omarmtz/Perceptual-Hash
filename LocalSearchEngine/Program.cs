using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
            //var agent = new FileAgent(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            var agent = new FileAgent(@"‪E:\");
            
            var watch = Stopwatch.StartNew();
            
            agent.InitializeIndexation();
            //agent.UpdateIndexation();
            watch.Stop();
            
            Console.WriteLine("Elapsed Time : {0}",watch.Elapsed);
            Console.ReadLine();
        }

        
    }
}
