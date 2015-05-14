using System.Drawing;
using System.IO;

namespace PHash
{
    interface IPerceptualImageHashFunction
    {
        byte[] GetHash(string imageFile);

        byte[] GetHash(Stream imageFile);

        byte[] GetHash(Bitmap imageFile);

    }
}
