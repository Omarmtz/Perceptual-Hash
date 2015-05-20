using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHash
{
    /// <summary>
    /// This interface shows the expected behaviour for all classes that implement the measure of similarity between media objects.
    /// </summary>
    /// <typeparam name="T">Type of media object</typeparam>
    interface IPerceptualHashing<T>
    {
        /// <summary>
        /// Gets the similarity based on two functions that define the similarity between two media objects
        /// 1) Hash Function 
        /// 2) Hash Comparision Function 
        /// </summary>
        /// <param name="mediaObjectA">Media Object A</param>
        /// <param name="mediaObjectB">Media Object B</param>
        /// <param name="hashFunc">Delegate Function that represents the process of hash generation
        /// Returns a N length BitArray representing resulting hash for each MediaObject T  
        /// </param>
        /// <param name="compareHashFunc">Delegate Function that represents the process of hash distance calculation
        /// Returns a float number representing hash distance between two N length BitArrays
        /// </param>
        /// <returns>Measured similarity between two objects</returns>
        float GetSimilarity(T mediaObjectA, T mediaObjectB, Func<T,BitArray> hashFunc, Func<BitArray,BitArray,float> compareHashFunc);

        /// <summary>
        /// Gets the similarity based on two functions that define the similarity between a media object against whole system database
        /// 1) Hash Function 
        /// 2) Hash Comparision Function 
        /// </summary>
        /// <param name="mediaObject">Media Object</param>
        /// <param name="hashFunc">Delegate Function that represents the process of hash generation
        /// Returns a N length BitArray representing resulting hash for each MediaObject T  
        /// </param>
        /// <param name="compareHashFunc">Delegate Function that represents the process of hash distance calculation
        /// Returns a float number representing hash distance between two N length BitArrays
        /// </param>
        /// <returns>Measured similarity against whole image database records</returns>
        float GetSimilarity(T mediaObject, Func<T, BitArray> hashFunc, Func<BitArray, BitArray, float> compareHashFunc);
        /// <summary>
        /// Get N length Hash based on the HashFunction Process applied to MediaObject
        /// </summary>
        /// <param name="mediaObject">Media Object</param>
        /// <param name="hashFunc">Delegate Function that represents the process of hash generation
        /// Returns a N length BitArray representing resulting hash for each MediaObject T  
        /// </param>
        /// <returns>N length BitArray Hash</returns>
        BitArray GetDigest(T mediaObject, Func<T, BitArray> hashFunc);

    }
}
