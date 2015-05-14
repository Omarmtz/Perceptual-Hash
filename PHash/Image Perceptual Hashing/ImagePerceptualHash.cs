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
        private const int ImageSize = 32;

        public override float GetSimilarity(Bitmap mediaObjectA, Bitmap mediaObjectB, Func<Bitmap, BitArray> hashFunc, Func<BitArray, BitArray, float> compareHashFunc)
        {
            var tmpA = PreProcessImage(mediaObjectA);
            var tmpB = PreProcessImage(mediaObjectB);

            var hashA = hashFunc(tmpA);
            var hashB = hashFunc(tmpB);

            return compareHashFunc(hashA, hashB);
        }

       

        public override float GetSimilarity(Bitmap mediaObject, Func<Bitmap, BitArray> hashFunc, Func<BitArray, BitArray, float> compareHashFunc)
        {
            //Access to DB Fast Comparing
            throw new NotImplementedException();
        }

        public override BitArray GetDigest(Bitmap mediaObject, Func<Bitmap, BitArray> hashFunc)
        {
            var tmp = PreProcessImage(mediaObject);

            return hashFunc(tmp);
        }

        private static Bitmap PreProcessImage(Bitmap mediaObjectA)
        {
            var tmp = ImageProcessing.Filters.BasicFilters.ResizeBitmap(mediaObjectA, ImageSize, ImageSize);
            tmp = (Bitmap)ImageProcessing.Filters.BasicFilters.GrayScale(tmp).Clone();
            return tmp;
        }
    }
}
