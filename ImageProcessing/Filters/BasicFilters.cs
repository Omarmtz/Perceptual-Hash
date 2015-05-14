using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Imaging.Filters;

namespace ImageProcessing.Filters
{
    public static class BasicFilters
    {
        public static Bitmap ResizeBitmap(Bitmap referenceBitmap,int width,int height)
        {
            var resizeNearestNeighbor = new ResizeNearestNeighbor(width,height);

            return resizeNearestNeighbor.Apply(referenceBitmap);
        }

        public static Bitmap GrayScale(Bitmap referenceBitmap)
        {
            // create grayscale filter (BT709)
            var filter = new Grayscale(0.2125, 0.7154, 0.0721);
            // apply the filter
            return filter.Apply(referenceBitmap);
        }
    }
}
