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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHash.Hash_Distance_Function
{
    /// <summary>
    /// The Hamming distance between two strings of equal length is the number of positions at which the corresponding symbols are different.
    /// In another way, it measures the minimum number of substitutions required to change one string into the other,
    /// or the minimum number of errors that could have transformed one string into the other.
    /// </summary>
    public class NormalizedHammingDistance : IPerceptualHashDistanceFunction
    {

        /// <summary>
        /// Gets the Hash normalized distance [0-1] from two bitarray hashes 
        /// </summary>
        /// <param name="hashA">Hash One</param>
        /// <param name="hashB">Hash Two</param>
        /// <returns>Float Number between [0...1] That represents the similarity between Media Objects
        /// 1 is the perceptually exact media object
        /// 0 is the perceptually distinct media object
        /// </returns>
        public float GetHashDistance(BitArray hashA, BitArray hashB)
        {
            if (hashA == null ||hashB == null|| hashA.Count != hashB.Count)
            {
                Console.WriteLine("Invalid Distance A - B");
                return -1;
            }
            var distance = 0;
            var distanceBitArray = hashA.Xor(hashB);

            //Get Differences between hashes
            for (var i = 0; i < distanceBitArray.Length; i++)
            {
                if (distanceBitArray[i])
                {
                    distance++;
                }
            }
            //Return normalized distance
            return 1 - (distance / (float)distanceBitArray.Length);

        }

        /// <summary>
        /// This method defines the comparison of distance between two hashes of 64 bit lenght
        /// It is implemented only for performance reasons
        /// ONLY 64 Bit perceptual hash.
        /// </summary>
        /// <param name="hashA">Hash One</param>
        /// <param name="hashB">Hash Two</param>
        /// <returns>Float Number between [0...1] That represents the similarity between Media Objects
        /// 1 is the perceptually exact media object
        /// 0 is the perceptually distinct media object
        /// </returns>
        public float GetHashDistance(ulong hashA, ulong hashB)
        {
            throw new NotImplementedException();
        }
    }
}
