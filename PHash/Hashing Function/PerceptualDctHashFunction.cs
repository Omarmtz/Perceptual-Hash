using System;
using System.Collections;
using System.Drawing;
using System.IO;

namespace PHash
{
    public class PerceptualDctHashFunction : IPerceptualImageHashFunction
    {
        public BitArray GetHash(string imageFile)
        {
            throw new NotImplementedException();
        }

        public BitArray GetHash(Stream imageFile)
        {
            throw new NotImplementedException();
        }

        public BitArray GetHash(Bitmap imageFile)
        {
            var dtcMatrix =ImageProcessing.ImageProcessingUtils.DctImageTransform.GetDctForwardTransform(imageFile);
            return CreateDigest(dtcMatrix);
        }

        private static BitArray CreateDigest(double[,] dctMatrix)
        {
            if (dctMatrix.GetLength(0) < 8 || dctMatrix.GetLength(1) < 8)
            {
                throw new Exception("Not supported : Image DCT is smaller than 8");
            }

            var mean = GetFirst8X8BlockMean(ref dctMatrix);
            var resultHash = new BitArray(64);
            int index = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    resultHash[index++] = dctMatrix[i, j] > mean;
                }
            }
            return resultHash;
        }

        private static double GetFirst8X8BlockMean(ref double[,] matrix)
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
