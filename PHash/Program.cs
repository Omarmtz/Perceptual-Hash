using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHash
{
    class Program
    {
        static void Main(string[] args)
        {
            ImagePerceptualHash phash = new ImagePerceptualHash();
            PerceptualDctHashFunction dtcFunction = new PerceptualDctHashFunction();
            var a = String.Empty;
            var watch = Stopwatch.StartNew();
            // the code that you want to measure comes here

            using (var fs = new FileStream(@"E:\Salzburg_from_Gaisberg_big_version.jpg", FileMode.Open, FileAccess.Read))
            {
                using (var img = Bitmap.FromStream(fs, true, false))
                {
                    var result = phash.GetDigest(new Bitmap(img), dtcFunction.GetHash);
                    
                    for (int i = 0; i < result.Length; i++)
                    {
                        a += result[i] ? "1" : "0";
                    }
                    
                }
            }
            watch.Stop();
            Console.WriteLine(a + " "+ watch.Elapsed);
            
            Console.ReadLine();
        }
    }
}
