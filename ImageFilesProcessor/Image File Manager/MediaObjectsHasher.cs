using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Linq;
using FileDataAccess;
using FileDataAccess.Database;
using PHash;
using PHash.Hash_Distance_Function;

namespace ImageFilesProcessor
{
    public class MediaObjectsHasher : IMediaObjectsHasher
    {
        /// <summary>
        /// Temporal Files Directory
        /// </summary>
        public static string TempFolder = Path.Combine(Path.GetTempPath(), "ImageSearchingTempFiles");
        /// <summary>
        /// Page size to get list of all indexed files in batches
        /// </summary>
        private const int PageDataBaseSize = 10000;

        private readonly ImagePerceptualHash phash;
        private readonly PerceptualDctHashFunction dtcFunction;
        private NormalizedHammingDistance normalizedHammingDistance;

        public MediaObjectsHasher()
        {
            phash = new ImagePerceptualHash();
            dtcFunction = new PerceptualDctHashFunction();
            normalizedHammingDistance = new NormalizedHammingDistance();
        }
        public void ScanDatabaseSystem()
        {
            var totalIndexed = DataBaseManager.GetTotalImagesWithoutPHash();

            var pagesCount = (totalIndexed / PageDataBaseSize);

            if (pagesCount * PageDataBaseSize < totalIndexed) pagesCount++;

            for (var i = 0; i < pagesCount; i++)
            {
                //Get indexed Result
                var results = DataBaseManager.GetImagesWithoutPHashPaged(i, PageDataBaseSize);

                results.AsParallel().ForAll((documentImage) =>
                {
                    BitArray pHash = null;

                    if (documentImage.IsWithinFile)
                    {
                        pHash = GetImageHash(Path.Combine(TempFolder, documentImage.TempKeyName));
                    }
                    else
                    {
                        var file = DataBaseManager.GetFile(documentImage.FileId);
                        pHash = GetImageHash(GetDocumentFullPathName(file));
                    }

                    documentImage.PFingerPrint = ToByteArray(pHash);

                    DataBaseManager.UpdateDocumentImage(documentImage);

                    Console.WriteLine("Image {0} Hashed", documentImage.Id);
                });
            }

        }

        /// <summary>
        /// Gets the full path of a document
        /// </summary>
        /// <param name="file">Document file</param>
        /// <returns>Full file path</returns>
        public static string GetDocumentFullPathName(DocumentFile file)
        {
            return Path.Combine(file.FolderPath, file.Name);
        }

        private BitArray GetImageHash(string filePath)
        {
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var img = Image.FromStream(fs, true, false))
                    {
                        return phash.GetDigest(new Bitmap(img), dtcFunction.GetHash);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[External] Image PHash Calculation Failed {0}:{1}", filePath, e.Message);
                return null;
            }
        }

        public static byte[] ToByteArray(BitArray bits)
        {
            try
            {
                int numBytes = bits.Count / 8;
                if (bits.Count % 8 != 0) numBytes++;

                byte[] bytes = new byte[numBytes];
                int byteIndex = 0, bitIndex = 0;

                for (int i = 0; i < bits.Count; i++)
                {
                    if (bits[i])
                        bytes[byteIndex] |= (byte)(1 << (7 - bitIndex));

                    bitIndex++;
                    if (bitIndex == 8)
                    {
                        bitIndex = 0;
                        byteIndex++;
                    }
                }

                return bytes;
            }
            catch (Exception)
            {
                Console.WriteLine("Hash Counln't be calculated returning Zero Hash");
                return new byte[1];
            }

        }


    }


}
