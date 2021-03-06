﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileDataAccess;

namespace ImageFilesProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            var imageHasher = new MediaObjectsHasher();
            var watch = Stopwatch.StartNew();
            imageHasher.ScanDatabaseSystem(new Progress<Tuple<int, int>>(e => Console.WriteLine("Test")));
            var results=imageHasher.GetImageSimilarities(@"E:\DCIM\PerfectlyClear_Camera\PC_20141101_225708.jpg", 85,MediaObjectsHasher.HashMethod.BlockMeanMethod4Overlapped);
            //imageHasher.GetImageSimilarities(@"E:\TestFolder\brookklyn.jpg", 1);
            watch.Stop();
            
            foreach (var archivo in results)
            {
                Process.Start(archivo.FilePath);
            }

            
            Console.WriteLine("Elapsed {0}",watch.Elapsed);
            
        }
    }
}
