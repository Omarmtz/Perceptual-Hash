using System;

namespace PHash
{
    public class PerceptualDctHashFunction : IPerceptualImageHashFunction
    {
        public byte[] GetHash(string imageFile)
        {
            throw new NotImplementedException();
        }

        public byte[] GetHash(System.IO.Stream imageFile)
        {
            throw new NotImplementedException();
        }

        public byte[] GetHash(System.Drawing.Bitmap imageFile)
        {
            throw new NotImplementedException();
        }
    }
}
