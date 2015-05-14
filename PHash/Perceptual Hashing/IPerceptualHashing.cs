using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHash
{
    interface IPerceptualHashing<in T>
    {
        float GetSimilarity(T mediaObjectA, T mediaObjectB, Delegate hashFunc, Delegate compareHashFunc);
        
        float GetSimilarity(T mediaObject, Delegate hashFunc, Delegate compareHashFunc);

        byte[] GetDigest(T mediaObject, Delegate hashFunc);

    }
}
