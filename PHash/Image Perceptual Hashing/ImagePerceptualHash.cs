using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHash
{
    public class ImagePerceptualHash: PerceptualHashing
    {
        public override float GetSimilarity(Bitmap mediaObjectA, Bitmap mediaObjectB, Func<Bitmap, BitArray> hashFunc, Func<BitArray, BitArray, float> compareHashFunc)
        {
            throw new NotImplementedException();
        }

        public override float GetSimilarity(Bitmap mediaObject, Func<Bitmap, BitArray> hashFunc, Func<BitArray, BitArray, float> compareHashFunc)
        {
            throw new NotImplementedException();
        }

        public override BitArray GetDigest(Bitmap mediaObject, Func<Bitmap, BitArray> hashFunc)
        {
            var tmp = ImageProcessing.Filters.BasicFilters.ResizeBitmap(mediaObject, 32, 32);
            tmp = (Bitmap)ImageProcessing.Filters.BasicFilters.GrayScale(tmp).Clone();

            return hashFunc(tmp);
        }
        
    }
}
