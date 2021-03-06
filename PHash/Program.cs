﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PHash.Hash_Distance_Function;

namespace PHash
{
    class Program
    {
        static void Main(string[] args)
        {
            var phash = new ImagePerceptualHash();
            var dtcFunction = new PerceptualDctHashFunction();
            var normalizedHammingDistance = new NormalizedHammingDistance();
            var blockmean = new BlockMeanHashFunction(BlockMeanHashFunction.MethodType.Method1UnOverlapped);
            
            float distance;

            Bitmap a = new Bitmap(Image.FromFile(@"E:\2.jpg"));
            Bitmap b = new Bitmap(Image.FromFile(@"E:\1.jpg"));


            var watch = Stopwatch.StartNew();
            //var array = phash.GetHash(a, blockmean.GetHash);
            // the code that you want to measure comes here
            //distance = phash.GetSimilarity(a, b, blockmean.GetHash, normalizedHammingDistance.GetHashDistance);
            Console.WriteLine("DCT {0} {1}", phash.GetSimilarity(a, b, dtcFunction.GetHash, normalizedHammingDistance.GetHashDistance), watch.Elapsed);
            Console.WriteLine("M1 {0} {1}", phash.GetSimilarity(a, b, blockmean.GetHash, normalizedHammingDistance.GetHashDistance), watch.Elapsed);
            blockmean.ChangeMethod(BlockMeanHashFunction.MethodType.Method2Overlapped);
            Console.WriteLine("M2 {0} {1}", phash.GetSimilarity(a, b, blockmean.GetHash, normalizedHammingDistance.GetHashDistance), watch.Elapsed);
            blockmean.ChangeMethod(BlockMeanHashFunction.MethodType.Method3UnOverlapped);
            Console.WriteLine("M3 {0} {1}", phash.GetSimilarity(a, b, blockmean.GetHash, normalizedHammingDistance.GetHashDistance), watch.Elapsed);
            blockmean.ChangeMethod(BlockMeanHashFunction.MethodType.Method4Overlapped);
            Console.WriteLine("M4 {0} {1}", phash.GetSimilarity(a, b, blockmean.GetHash, normalizedHammingDistance.GetHashDistance), watch.Elapsed);

            
            watch.Stop();

            
            Console.ReadLine();
        }
    }
}
