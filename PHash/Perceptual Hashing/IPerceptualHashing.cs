using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHash
{
    interface IPerceptualHashing<T>
    {

        float GetSimilarity(T mediaObjectA, T mediaObjectB, Func<T,BitArray> hashFunc, Func<BitArray,BitArray,float> compareHashFunc);

        float GetSimilarity(T mediaObject, Func<T, BitArray> hashFunc, Func<BitArray, BitArray, float> compareHashFunc);

        BitArray GetDigest(T mediaObject, Func<T, BitArray> hashFunc);

    }
}
