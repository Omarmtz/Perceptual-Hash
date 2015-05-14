using System.Collections;
using System.Drawing;
using System.IO;

namespace PHash
{
    interface IPerceptualImageHashFunction
    {
        BitArray GetHash(string imageFile);

        BitArray GetHash(Stream imageFile);

        BitArray GetHash(Bitmap imageFile);

    }
}
