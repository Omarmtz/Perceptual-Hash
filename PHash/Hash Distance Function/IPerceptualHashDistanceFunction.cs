using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHash.Hash_Distance_Function
{
    interface IPerceptualHashDistanceFunction
    {
        float GetHashDistance(byte[] hashA, byte hashB);

        float GetHashDistance(ulong hashA, ulong hashB);
    }
}
