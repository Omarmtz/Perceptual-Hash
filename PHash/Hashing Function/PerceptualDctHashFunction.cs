// <copyright>
// Copyright (c) 2015, All Right Reserved
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// </copyright>
// <author>Omar Martínez Rosas</author>
// <email>omack47@gmail.com</email>
// <date>3-07-2015</date>

using System;
using System.Collections;
using System.Drawing;
using System.IO;

namespace PHash
{
    /// <summary>
    // The  DCT, like any Fourier-related transform, expresses a function or signal
    // (a sequence of finitely many data points) in terms of a sum of sinusoids with
    // different frequencies and amplitudes.
    // The DCT uses only cosine function.
    /// </summary>
    public class PerceptualDctHashFunction : IPerceptualImageHashFunction
    {
        /// <summary>
        /// Get hash from an image file in the system
        /// </summary>
        /// <param name="imageFile">File path</param>
        /// <returns>Bitarray value representing N length Hash</returns>
        public BitArray GetHash(string imageFile)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Get hash from any kind of stream containing image data
        /// </summary>
        /// <param name="imageFile">Image stream</param>
        /// <returns>Bitarray value representing N length Hash</returns>
        public BitArray GetHash(Stream imageFile)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Get hash from an image bitmap
        /// </summary>
        /// <param name="imageFile">Bitmap of image</param>
        /// <returns>Bitarray value representing N length Hash</returns>
        public BitArray GetHash(Bitmap imageFile)
        {
            //Manual DCT Implementation
            //var dctMatrix = ImageProcessing.ImageProcessingUtils.DctImageTransform.GetDctForwardTransform(imageFile);
            var dctMatrix = ImageProcessing.ImageProcessingUtils.DctImageTransform.GetFastDctForwardTransform(imageFile);

            return CreateSign(dctMatrix);
        }
        /// <summary>
        /// Create Sign from an 32x32 DCT Image Matrix
        /// </summary>
        /// <param name="dctMatrix">Discrete Cosine Transformed 32x32 Matrix</param>
        /// <returns></returns>
        private static BitArray CreateSign(double[,] dctMatrix)
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
        /// <summary>
        /// Get the mean of the first block of 8x8 in the DCT Matrix Due to :
        /// "The dct hash is based on the low 2D DCT coefficients starting at the second from lowest, leaving out the first DC term.
        /// This excludes completely flat image information (i.e. solid colors) from being included in the hash description."
        /// </summary>
        /// <param name="dctMatrix">Discrete Cosine Transformed 32x32 Matrix</param>
        /// <returns></returns>
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
