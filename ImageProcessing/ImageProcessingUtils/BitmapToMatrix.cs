using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Imaging.Converters;

namespace ImageProcessing.ImageProcessingUtils
{
    public static class BitmapToMatrix
    {
        public static double[,] ConvertBitmapToMatrix(Bitmap reference)
        {
            // Create the converter to convert the image to a
            //  matrix containing only values between 0 and 255 
            var converter = new ImageToMatrix(min: 0, max: 255);
            double[,] referenceMatrix;
            converter.Convert(reference, out referenceMatrix);
            
            return referenceMatrix;
        }

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
