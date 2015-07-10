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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ImageProcessing.Filters;
using ImageProcessing.ImageProcessingUtils;

namespace PHash
{
    public class BlockMeanHashFunction : IPerceptualImageHashFunction
    {
        public enum MethodType
        {
            Method1UnOverlapped,
            Method2Overlapped,
            Method3UnOverlapped,
            Method4Overlapped,
        }

        public BlockMeanHashFunction(MethodType methodType)
        {
            this._selectedMethod = methodType;
        }

        private MethodType _selectedMethod;

        public void ChangeMethod(MethodType method)
        {
            this._selectedMethod = method;
        }

        public BitArray GetHash(string imageFile)
        {
            throw new NotImplementedException();
        }

        public BitArray GetHash(System.IO.Stream imageFile)
        {
            throw new NotImplementedException();
        }

        public BitArray GetHash(Bitmap imageFile)
        {
            switch (_selectedMethod)
            {
                case MethodType.Method1UnOverlapped:
                        return CreateSignMethod1(imageFile);
                case MethodType.Method2Overlapped:
                        return CreateSignMethod2(imageFile);
                case MethodType.Method3UnOverlapped:
                        return CreateSignMethod3(imageFile);
                case MethodType.Method4Overlapped:
                        return CreateSignMethod4(imageFile);
            }
            return null;
        }

        private BitArray CreateSignMethod4(Bitmap imageFile)
        {
            var hashList = new List<BitArray>();
            for (int i = 0; i <= 345; i += 15)
            {
                hashList.Add(
                    CreateSignMethod2(
                    BasicFilters.RotateBitmap(imageFile, i)));
            }

            var resultHash = new BitArray(hashList.Sum(e => e.Length));

            int index = 0;
            foreach (var bitArray in hashList)
            {
                for (int i = 0; i < bitArray.Length; i++)
                {
                    resultHash[index] = bitArray[i];
                    index++;
                }
            }
            return resultHash;
        }

        private BitArray CreateSignMethod3(Bitmap imageFile)
        {
            var hashList = new List<BitArray>();
            for (int i = 0; i <= 345; i += 15)
            {
                hashList.Add(
                    CreateSignMethod1(
                    BasicFilters.RotateBitmap(imageFile, i)));
            }

            var resultHash = new BitArray(hashList.Sum(e => e.Length));

            int index = 0;
            foreach (var bitArray in hashList)
            {
                for (int i = 0; i < bitArray.Length; i++)
                {
                    resultHash[index] = bitArray[i];
                    index++;
                }
            }
            return resultHash;
        }

        private BitArray CreateSignMethod2(Bitmap imageFile)
        {
            if (imageFile == null) throw new ArgumentNullException("imageFile");
            var matrixReference = BitmapToMatrix.ConvertBitmapToMatrix(imageFile);

            IList<double> meanBlockList;

            var generatedBlocks = GetOverlappedBlocks(matrixReference, matrixReference.GetLength(0) / 4);

            var totalBlocksMean = GetBlocksMean(generatedBlocks, out meanBlockList);

            //Make Hash Byte convertible by addign requiered bits to form the last byte
            var length = meanBlockList.Count;
            while (length%8 != 0)
            {
                length++;
            }

            var resultHash = new BitArray(length);

            for (int i = 0; i < meanBlockList.Count; i++)
            {
                resultHash[i] = meanBlockList[i] >= totalBlocksMean;
            }

            return resultHash;
        }
        private BitArray CreateSignMethod1(Bitmap imageFile)
        {
            if (imageFile == null) throw new ArgumentNullException("imageFile");
            var matrixReference = BitmapToMatrix.ConvertBitmapToMatrix(imageFile);

            IList<double> meanBlockList;

            var generatedBlocks = GenerateUnoverlappedBlocks(matrixReference, matrixReference.GetLength(0) / 8);

            var totalBlocksMean = GetBlocksMean(generatedBlocks, out meanBlockList);

            var resultHash = new BitArray(meanBlockList.Count);

            for (int i = 0; i < meanBlockList.Count; i++)
            {
                resultHash[i] = meanBlockList[i] >= totalBlocksMean;
            }

            return resultHash;
        }

        #region Block Methods
        private static IList<double[,]> GenerateUnoverlappedBlocks(double[,] matrixReference, int blockSize)
        {
            if (matrixReference == null) throw new ArgumentNullException("matrixReference");

            if (matrixReference.GetLength(0) % 2 != 0 || matrixReference.GetLength(1) != matrixReference.GetLength(0))
            {
                throw new Exception("Non Square matrix are not allowed in Unoverlapped Block Mean Generation");
            }

            var blockList = new List<double[,]>();

            for (var i = 0; i <= matrixReference.GetLength(0)-blockSize; i += blockSize)
            {
                for (var j = 0; j <= matrixReference.GetLength(1) - blockSize; j += blockSize)
                {
                    var block = GenerateBlock(matrixReference, blockSize, i, j);
                    blockList.Add(block);
                }
            }

            return blockList;
        }

        private static IList<double[,]> GetOverlappedBlocks(double[,] matrixReference, int blockSize)
        {
            if (matrixReference.GetLength(0) % 2 != 0 || matrixReference.GetLength(0) != matrixReference.GetLength(1))
            {
                throw new Exception("Matrix must be Squared");
            }

            if (matrixReference.GetLength(0) % blockSize != 0)
            {
                throw new Exception("blockSize must fit in Matrix");
            }

            var blockList = new List<double[,]>();
            var halfSize = (blockSize / 2);
            for (var i = 0; i < matrixReference.GetLength(0) - halfSize; i += halfSize)
            {
                for (var j = 0; j < matrixReference.GetLength(1) - halfSize; j += halfSize)
                {
                    var block = GenerateBlock(matrixReference, blockSize, i, j);
                    blockList.Add(block);
                }
            }

            return blockList;
        }

        private static double[,] GenerateBlock(double[,] matrixReference, int blockLength, int i, int j)
        {
            var block = new double[blockLength, blockLength];

            for (var k = 0; k < blockLength; k++)
            {
                for (var l = 0; l < blockLength; l++)
                {
                    block[k, l] = matrixReference[i + k, j + l];
                }
            }
            return block;
        }

        private static double GetBlockMean(double[,] block)
        {
            var width = block.GetLength(0);
            var height = block.GetLength(1);
            double mean = 0;
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    mean += block[i, j];
                }
            }
            return mean / (double)(width + height);
        }

        private static double GetBlocksMean(IList<double[,]> blockList, out IList<double> meanBlockList)
        {
            meanBlockList = blockList.Select(GetBlockMean).ToList();

            return meanBlockList.Sum() / (double)blockList.Count;
        }
        #endregion
    }
}
