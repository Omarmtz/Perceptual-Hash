using System;
using System.Collections.Generic;
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

            using (var fs = new FileStream(@"E:\Alyson_Hannigan_200512.jpg", FileMode.Open, FileAccess.Read))
            {
                using (var img = Bitmap.FromStream(fs, true, false))
                {
                    var result = phash.GetDigest(new Bitmap(img), dtcFunction.GetHash);
                    var a = String.Empty;
                    for (int i = 0; i < result.Length; i++)
                    {
                        a += result[i] ? "1" : "0";
                    }
                    Console.WriteLine(a);
                }
            }
            Console.ReadLine();
        }
    }
}
