using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHash
{
    public abstract class PerceptualHashing : IPerceptualHashing<Bitmap>
    {
        public abstract float GetSimilarity(Bitmap mediaObjectA, Bitmap mediaObjectB, Delegate hashFunc, Delegate compareHashFunc);

        public abstract float GetSimilarity(string fileObjectA, string fileObjectB, Delegate hashFunc, Delegate compareHashFunc);

        public abstract float GetSimilarity(Bitmap mediaObject, Delegate hashFunc, Delegate compareHashFunc);

        public abstract float GetSimilarity(string fileObject, Delegate hashFunc, Delegate compareHashFunc);

        public abstract byte[] GetDigest(Bitmap mediaObject, Delegate hashFunc);
    }
}
