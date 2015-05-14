using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHash
{
    public abstract class PerceptualHashing : IPerceptualHashing<Bitmap>
    {
        public abstract float GetSimilarity(Bitmap mediaObjectA, Bitmap mediaObjectB, Func<Bitmap, BitArray> hashFunc,
            Func<BitArray, BitArray, float> compareHashFunc);

        public abstract float GetSimilarity(Bitmap mediaObject, Func<Bitmap, BitArray> hashFunc,
            Func<BitArray, BitArray, float> compareHashFunc);

        public abstract BitArray GetDigest(Bitmap mediaObject, Func<Bitmap, BitArray> hashFunc);
    }
}
