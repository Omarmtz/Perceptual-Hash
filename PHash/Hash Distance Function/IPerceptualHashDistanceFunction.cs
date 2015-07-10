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
    /// This interface shows the expected behaviour for all classes that implement the calculation of Hash distances.
    /// </summary>
    interface IPerceptualHashDistanceFunction
    {
        /// <summary>
        /// This method defines the comparison of distance between two hashes
        /// </summary>
        /// <param name="hashA">Defines Hash One</param>
        /// <param name="hashB">Defines Hash Two</param>
        /// <returns>Float Number between [0...1] That represents the similarity between Media Objects
        /// 1 is the perceptually exact media object
        /// 0 is the perceptually distinct media object
        /// </returns>
        float GetHashDistance(BitArray hashA, BitArray hashB);

        /// <summary>
        /// This method defines the comparison of distance between two hashes of 64 bit lenght
        /// It is implemented only for performance reasons
        /// </summary>
        /// <param name="hashA">Defines Hash One</param>
        /// <param name="hashB">Defines Hash Two</param>
        /// <returns>Float Number between [0...1] That represents the similarity between Media Objects
        /// 1 is the perceptually exact media object
        /// 0 is the perceptually distinct media object
        /// </returns>
        float GetHashDistance(ulong hashA, ulong hashB);
    }
}
