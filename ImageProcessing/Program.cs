using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ImageProcessing.ImageProcessingUtils;

namespace ImageProcessing
{
    class Program
    {
        private static void Main(string[] args)
        {
            
        }

        public static void PrintMatrix(ref double[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write("{0} ", matrix[i, j].ToString("#.##"));
                }
                Console.WriteLine();
            }
        }
    }
}
