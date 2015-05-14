using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHash
{
    public class ImagePerceptualHash: PerceptualHashing
    {
        public override float GetSimilarity(System.Drawing.Bitmap mediaObjectA, System.Drawing.Bitmap mediaObjectB, Delegate hashFunc, Delegate compareHashFunc)
        {
            throw new NotImplementedException();
        }

        public override float GetSimilarity(string fileObjectA, string fileObjectB, Delegate hashFunc, Delegate compareHashFunc)
        {
            throw new NotImplementedException();
        }

        public override float GetSimilarity(System.Drawing.Bitmap mediaObject, Delegate hashFunc, Delegate compareHashFunc)
        {
            throw new NotImplementedException();
        }

        public override float GetSimilarity(string fileObject, Delegate hashFunc, Delegate compareHashFunc)
        {
            throw new NotImplementedException();
        }

        public override byte[] GetDigest(System.Drawing.Bitmap mediaObject, Delegate hashFunc)
        {
            throw new NotImplementedException();
        }
    }
}
