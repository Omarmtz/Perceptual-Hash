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
using System.Text;
using System.Threading.Tasks;

namespace PHash
{
    /// <summary>
    /// This Abstract Class shows the expected behaviour for all classes that implement P.Hash Image Similarity Calculation
    /// </summary>
    /// <typeparam name="T">Type of image</typeparam>
    public abstract class PerceptualHashing : IPerceptualHashing<Bitmap>
    {
        /// <summary>
        /// Gets the similarity based on two functions that define the similarity between two images
        /// 1) Hash Function 
        /// 2) Hash Comparision Function 
        /// </summary>
        /// <param name="mediaObjectA">image A</param>
        /// <param name="mediaObjectB">image B</param>
        /// <param name="hashFunc">Delegate Function that represents the process of hash generation
        /// Returns a N length BitArray representing resulting hash for each image  
        /// </param>
        /// <param name="compareHashFunc">Delegate Function that represents the process of hash distance calculation
        /// Returns a float number representing hash distance between two N length BitArrays
        /// </param>
        /// <returns>Measured similarity between two objects</returns>
        public abstract float GetSimilarity(Bitmap mediaObjectA, Bitmap mediaObjectB, Func<Bitmap, BitArray> hashFunc,
            Func<BitArray, BitArray, float> compareHashFunc);
        
        /// <summary>
        /// Get N length Hash based on the HashFunction Process applied to an image
        /// </summary>
        /// <param name="mediaObject">image</param>
        /// <param name="hashFunc">Delegate Function that represents the process of hash generation
        /// Returns a N length BitArray representing resulting hash for each image  
        /// </param>
        /// <returns>N length BitArray Hash</returns>
        public abstract BitArray GetHash(Bitmap mediaObject, Func<Bitmap, BitArray> hashFunc);
    }
}
