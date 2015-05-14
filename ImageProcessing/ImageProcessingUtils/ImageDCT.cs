using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageProcessing.ImageProcessingUtils
{
    public static class DctImageTransform
    {
        public static double[,] GetDctForwardTransform(Bitmap reference)
        {
            var referenceMatrix = TransformBitmapToMatrix(reference);

            return GetDctForwardTransform(ref referenceMatrix);
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

        private static double[,] TransformBitmapToMatrix(Bitmap image)
        {
            using (image)
            {
                var originalData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                   ImageLockMode.ReadWrite, image.PixelFormat);
                var result = new double[image.Height, image.Width];

                unsafe
                {
                    for (var i = 0; i < image.Height; i++)
                    {
                        var irow = (byte*)originalData.Scan0 + (i * originalData.Stride);
                        for (var j = 0; j < image.Width; j++)
                        {
                            result[i, j] = irow[j];
                        }
                    }
                }
                image.UnlockBits(originalData);
                return result;
            }
        }

        public static double GetFirst8X8BlockMean(ref double[,] matrix)
        {
            double value = 0;
            if (matrix.GetLength(0) != 32 || matrix.GetLength(1) != 32)
            {
                throw new Exception("Image must be width=height = 32");
            }

            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    if (i == 0 && j == 0)
                        continue;
                    value += matrix[i, j];
                }
            }

            return value / (double)63;
        }
    }
}
