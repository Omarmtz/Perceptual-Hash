using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Imaging.Filters;

namespace ImageProcessing.Filters
{
    /// <summary>
    /// Basic images filters 
    /// </summary>
    public static class BasicFilters
    {
        /// <summary>
        /// Process crop process of a given image using NearestNeightbor Algorithm
        /// </summary>
        /// <param name="referenceBitmap">Reference Image</param>
        /// <param name="width">New width of image</param>
        /// <param name="height">New height of image</param>
        /// <returns>Cropped/Resized Image</returns>
        public static Bitmap ResizeBitmap(Bitmap referenceBitmap,int width,int height)
        {
            var resizeNearestNeighbor = new ResizeNearestNeighbor(width,height);

            return resizeNearestNeighbor.Apply(referenceBitmap);
        }
        /// <summary>
        /// Apply GrayScale using Standard filter (BT709) http://www.itu.int/pub/R-REC 
        /// Also Known as "Luma":
        /// Luma represents the brightness in an image (the "black-and-white" or achromatic portion of the image).
        /// Luma is typically paired with chrominance.
        /// Luma represents the achromatic image, while the chroma components represent the color information.
        /// Converting R'G'B' sources (such as the output of a 3CCD camera) into luma and chroma allows for chroma subsampling:
        /// because human vision has finer spatial sensitivity to luminance ("black and white") differences than chromatic differences,
        ///  video systems can store chromatic information at lower resolution, optimizing perceived detail at a particular bandwidth.
        /// </summary>
        /// <param name="referenceBitmap">Reference Image Bitmap</param>
        /// <returns>Bitmap in GrayScale</returns>
        public static Bitmap GrayScale(Bitmap referenceBitmap)
        {
            // create grayscale filter (BT709)
            var filter = new Grayscale(0.2125, 0.7154, 0.0721);
            // apply the filter
            return filter.Apply(referenceBitmap);
        }
    }
}
