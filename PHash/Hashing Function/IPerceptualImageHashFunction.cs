using System.Collections;
using System.Drawing;
using System.IO;

namespace PHash
{
    /// <summary>
    /// This interface shows the expected behaviour for all classes that implement the calculation of Hash in images
    /// Hash functions are frequently called message digest functions.
    /// Their purpose is to extract a fixed-length bit string from a message (image, documents, etc.).
    /// Hash functions have found varied applications in various cryptographic, compiler and database search applications.
    /// In cryptography, hash functions are typically used for digital signatures to authenticate the message being sent so that the recipient can verify its source. 
    /// </summary>
    interface IPerceptualImageHashFunction
    {
        /// <summary>
        /// Get hash from an image file in the system
        /// </summary>
        /// <param name="imageFile">File path</param>
        /// <returns>Bitarray value representing N length Hash</returns>
        BitArray GetHash(string imageFile);
        /// <summary>
        /// Get hash from any kind of stream containing image data
        /// Any class that inherits from Stream
        /// Memory Stream, File Stream ....
        /// </summary>
        /// <param name="imageFile">Stream of image file</param>
        /// <returns>Bitarray value representing N length Hash</returns>
        BitArray GetHash(Stream imageFile);
        /// <summary>
        /// Get hash from an image bitmap
        /// </summary>
        /// <param name="imageFile">Bitmap of image</param>
        /// <returns>Bitarray value representing N length Hash</returns>
        BitArray GetHash(Bitmap imageFile);

    }
}
