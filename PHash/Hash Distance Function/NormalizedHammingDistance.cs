using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHash.Hash_Distance_Function
{
    public class NormalizedHammingDistance:IPerceptualHashDistanceFunction
    {
        
        public float GetHashDistance(BitArray hashA, BitArray hashB)
        {
            var distance = 0;
            var distanceBitArray=hashA.Xor(hashB);
            
            for (var i = 0; i < distanceBitArray.Length; i++)
            {
                if (distanceBitArray[i])
                {
                    distance++;
                }
            }

            return 1-(distance/(float)distanceBitArray.Length);

        }

        public float GetHashDistance(ulong hashA, ulong hashB)
        {
            throw new NotImplementedException();
        }
    }
}
