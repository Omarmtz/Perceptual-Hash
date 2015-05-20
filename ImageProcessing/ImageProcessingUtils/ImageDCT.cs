using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using Accord.Imaging.Converters;

namespace ImageProcessing.ImageProcessingUtils
{
    /// <summary>
    /// Get the DCT of an image
    /// </summary>
    public static class DctImageTransform
    {
        /// <summary>
        /// Process the forward DCT of an image
        /// </summary>
        /// <param name="reference">Image reference Bitmap</param>
        /// <returns>NxN Forward Resulting DCT Double Matrix</returns>
        public static double[,] GetDctForwardTransform(Bitmap reference)
        {
            var referenceMatrix = BitmapToMatrix.ConvertBitmapToMatrix(reference);

            return GetDctForwardTransform(ref referenceMatrix);
        }
        /// <summary>
        /// Process the fast-forward DCT of an image (FFT - DCT)
        /// </summary>
        /// <param name="reference">Image reference Bitmap</param>
        /// <returns>NxN Forward Resulting DCT Double Matrix</returns>
        public static double[,] GetFastDctForwardTransform(Bitmap reference)
        {
            var referenceMatrix = BitmapToMatrix.ConvertBitmapToMatrix(reference);

            Accord.Math.CosineTransform.DCT(referenceMatrix);

            return referenceMatrix;
        }

        /// <summary>
        /// Process the forward DCT of a double matrix
        /// </summary>
        /// <param name="reference">NxN Double Matrix</param>
        /// <returns></returns>
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
        /// <summary>
        /// Refers to the values of alpha function in dt transform 
        /// [The DCT Transform Matrix]
        /// http://www.mathworks.com/help/images/discrete-cosine-transform.html
        /// </summary>
        /// <param name="u">U Index Function Value</param>
        /// <param name="n">V Index Function Value</param>
        /// <returns>Resulting value of Function operation</returns>
        private static double GetAlpha(int u, int n)
        {
            return u == 0 ? Math.Sqrt(1 / (double)n) : Math.Sqrt(2 / (double)n);
        }

        /// <summary>
        /// Cosine Transform of given N Matrix Row
        /// </summary>
        /// <param name="reference">Reference Matrix</param>
        /// <param name="u">U Index Function Value</param>
        /// <param name="n">N Index Matrix Value</param>
        /// <returns>Resulting value of Function operation</returns>
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
