using System;
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
            float distance;

            Bitmap a = new Bitmap(Image.FromFile(@"E:\DCIM\.thumbnails\1417064109701.jpg"));
            Bitmap b = new Bitmap(Image.FromFile(@"E:\2.png"));

            var array =phash.GetDigest(a, dtcFunction.GetHash);
            string text = string.Empty;

            foreach (var VARIABLE in array)
            {
                if ((bool) VARIABLE == true)
                {
                    text += "1";
                }
                else
                {
                    text += "0";
                }
            }

            var watch = Stopwatch.StartNew();
            
            // the code that you want to measure comes here

            distance=phash.GetSimilarity(a, b, dtcFunction.GetHash, normalizedHammingDistance.GetHashDistance);
            
            watch.Stop();

            Console.WriteLine("{0} {1}", distance, watch.Elapsed);

            Console.ReadLine();
        }
    }
}
