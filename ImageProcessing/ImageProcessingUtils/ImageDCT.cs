using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using Accord.Imaging.Converters;

namespace ImageProcessing.ImageProcessingUtils
{
    public static class DctImageTransform
    {
        public static double[,] GetDctForwardTransform(Bitmap reference)
        {
            var referenceMatrix = ConvertBitmapToMatrix(reference);

            return GetDctForwardTransform(ref referenceMatrix);
        }

        private static double[,] ConvertBitmapToMatrix(Bitmap reference)
        {
            // Create the converter to convert the image to a
            //  matrix containing only values between 0 and 255 
            var converter = new ImageToMatrix(min: 0, max: 255);
            double[,] referenceMatrix;
            converter.Convert(reference, out referenceMatrix);
            return referenceMatrix;
        }

        public static double[,] GetFastDctForwardTransform(Bitmap reference)
        {
            var referenceMatrix = ConvertBitmapToMatrix(reference);

            Accord.Math.CosineTransform.DCT(referenceMatrix);

            return referenceMatrix;
        }

        public static double[,] GetDctForwardTransform(ref double[,] reference)
        {
            var transform = new double[reference.GetLength(0), reference.GetLength(1)];
            var n = reference.GetLength(0);

            for (var u = 0; u < transform.GetLength(0); u++)
            {
                for (var v = 0; v < transform.GetLength(1); v++)
                {
                    transform[u, v] = GetAlpha(u, n) * GetAlpha(v, n)
                        * CosineTransform(ref reference, u, v);
                }
            }
            return transform;
        }

        private static double GetAlpha(int u, int n)
        {
            return u == 0 ? Math.Sqrt(1 / (double)n) : Math.Sqrt(2 / (double)n);
        }

        private static double CosineTransform(ref double[,] reference, int u, int v)
        {
            double n = reference.GetLength(0);
            double sum = 0;
            for (var x = 0; x < reference.GetLength(0); x++)
            {
                for (var y = 0; y < reference.GetLength(1); y++)
                {
                    sum += reference[x, y]
                        * Math.Cos(((Math.PI * ((2 * x) + 1) * u) / (2 * n)))
                        * Math.Cos(((Math.PI * ((2 * y) + 1) * v) / (2 * n)));
                }
            }
            return sum;
        }
    }
}
