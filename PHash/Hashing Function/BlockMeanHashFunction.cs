using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHash.Hashing_Function
{
    public class BlockMeanHashFunction:IPerceptualImageHashFunction
    {
        public BitArray GetHash(string imageFile)
        {
            throw new NotImplementedException();
        }

        public BitArray GetHash(System.IO.Stream imageFile)
        {
            throw new NotImplementedException();
        }

        public BitArray GetHash(System.Drawing.Bitmap imageFile)
        {
            var refMatrix = ImageProcessing.ImageProcessingUtils.BitmapToMatrix.ConvertBitmapToMatrix(imageFile);
            return CreateSign(refMatrix);
        }

        private BitArray CreateSign(double[,] matrixReference)
        {
            if (matrixReference == null) throw new ArgumentNullException("matrixReference");

            IList<double> meanBlockList;
            
            var generatedBlocks = GenerateUnOverlappedBlocks(matrixReference, 64);
            
            var totalBlocksMean = GetBlocksMean(generatedBlocks,out meanBlockList);

            var resultHash = new BitArray(64);

            for (int i = 0; i < meanBlockList.Count; i++)
            {
                resultHash[i] = meanBlockList[i] >= totalBlocksMean;
            }

            return resultHash;
        }

        #region Block Methods
        private static IList<double[,]> GenerateUnOverlappedBlocks(double[,] matrixReference, int numBlocks)
        {
            if (matrixReference == null) throw new ArgumentNullException("matrixReference");

            if (matrixReference.GetLength(0) % 2 != 0 || matrixReference.GetLength(1) != matrixReference.GetLength(0))
            {
                throw new Exception("Non Square matrix are not allowed in Unoverlapped Block Mean Generation");
            }

            var blockLength = (numBlocks/matrixReference.GetLength(0))*2;

            var blockList =new List<double[,]>();

            for (var i = 0; i < matrixReference.GetLength(0); i += blockLength)
            {
                for (var j = 0; j < matrixReference.GetLength(1); j += blockLength)
                {
                    var block = GenerateBlock(matrixReference, blockLength, i, j);
                    blockList.Add(block);
                }
            }
            
            return blockList;
        }

        private static double[,] GenerateBlock(double[,] matrixReference,int blockLength,int i, int j)
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
