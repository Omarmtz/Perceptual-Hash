using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageFilesProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            var imageHasher = new MediaObjectsHasher();
            var watch = Stopwatch.StartNew();
            imageHasher.ScanDatabaseSystem();
            //imageHasher.GetImageSimilarities(@"E:\DCIM\PerfectlyClear_Camera\PC_20141101_225708.jpg", 75);
            //imageHasher.GetImageSimilarities(@"E:\TestFolder\brookklyn.jpg", 1);
            watch.Stop();
            Console.WriteLine("Elapsed {0}",watch.Elapsed);
            
        }
    }
}
