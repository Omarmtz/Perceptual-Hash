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
            //var agent = new FileAgent(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            var agent = new FileAgent(@"c:\");
            agent.InitializeIndexation();
            //agent.CheckForUpdates();
            
        }

        
    }
}
