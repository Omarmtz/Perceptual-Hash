using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Imaging.Converters;

namespace ImageProcessing.ImageProcessingUtils
{
    /// <summary>
    /// Converts any bitmap to a float or double Matrix for further image operations
    /// </summary>
    public static class BitmapToMatrix
    {
        /// <summary>
        /// Converts a Bitmap Image to a N*M Double Data Type Values Matrix
        /// </summary>
        /// <param name="reference">Image Bitmap Reference</param>
        /// <returns></returns>
        public static double[,] ConvertBitmapToMatrix(Bitmap reference)
        {
            // Create the converter to convert the image to a
            //  matrix containing only values between 0 and 255 
            var converter = new ImageToMatrix(min: 0, max: 255);
            double[,] referenceMatrix;
            converter.Convert(reference, out referenceMatrix);
            
            return referenceMatrix;
        }
        /// <summary>
        /// Converts a Bitmap Image to a N*M Float Data Type Values Matrix
        /// </summary>
        /// <param name="reference">Image Bitmap Reference</param>
        /// <returns></returns>
        public static float[,] ConvertBitmapToFloatMatrix(Bitmap reference)
        {
            // Create the converter to convert the image to a
            //  matrix containing only values between 0 and 255 
            var converter = new ImageToMatrix(min: 0, max: 255);
            float[,] referenceMatrix;
            converter.Convert(reference, out referenceMatrix);

            return referenceMatrix;
        }
    }
}
