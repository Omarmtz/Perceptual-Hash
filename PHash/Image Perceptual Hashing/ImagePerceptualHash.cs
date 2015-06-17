using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHash
{
    /// <summary>
    /// Implements the image P.Hash
    /// </summary>
    public class ImagePerceptualHash: PerceptualHashing
    {
        /// <summary>
        /// Defines the Crop Image Size for the DCT Processing
        /// </summary>
        private const int ImageSize = 32;

        /// <summary>
        /// Get similarity of two images
        /// </summary>
        /// <param name="mediaObjectA">Image A</param>
        /// <param name="mediaObjectB">Image B</param>
        /// <param name="hashFunc">Hash Calculation Function</param>
        /// <param name="compareHashFunc">Hash Distance Calculation Function</param>
        /// <returns>Similarity of two images</returns>
        public override float GetSimilarity(Bitmap mediaObjectA, Bitmap mediaObjectB, Func<Bitmap, BitArray> hashFunc, Func<BitArray, BitArray, float> compareHashFunc)
        {
            //Preprocessing Images
            var tmpA = PreProcessImage(mediaObjectA);
            var tmpB = PreProcessImage(mediaObjectB);
            //Calculation of hashfunction
            var hashA = hashFunc(tmpA);
            var hashB = hashFunc(tmpB);
            //Calculation of similarity between hashes
            return compareHashFunc(hashA, hashB);
        }
       
        /// <summary>
        /// Get N length Hash based on the HashFunction Process applied to an image
        /// </summary>
        /// <param name="mediaObject">Image</param>
        /// <param name="hashFunc">Delegate Function that represents the process of hash generation
        /// Returns a N length BitArray representing resulting hash for each image  
        /// </param>
        /// <returns>N length BitArray Hash</returns>
        public override BitArray GetHash(Bitmap mediaObject, Func<Bitmap, BitArray> hashFunc)
        {
            var tmp = PreProcessImage(mediaObject);

            return hashFunc(tmp);
        }
        /// <summary>
        /// Image Preprocessing previous Hash Calculation
        /// </summary>
        /// <param name="mediaObjectA">Image</param>
        /// <returns></returns>
        private static Bitmap PreProcessImage(Bitmap mediaObjectA)
        {
            var tmp = ImageProcessing.Filters.BasicFilters.ResizeBitmap(mediaObjectA, ImageSize, ImageSize);
            tmp = (Bitmap)ImageProcessing.Filters.BasicFilters.GrayScale(tmp).Clone();
            return tmp;
        }
    }
}
