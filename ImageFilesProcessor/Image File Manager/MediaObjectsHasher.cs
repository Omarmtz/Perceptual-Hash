using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        private const int PageDataBaseSize = 1000;

        private readonly ImagePerceptualHash _phash;
        private readonly PerceptualDctHashFunction _dtcFunction;
        private readonly NormalizedHammingDistance _normalizedHammingDistance;
        /// <summary>
        /// 
        /// </summary>
        public MediaObjectsHasher()
        {
            _phash = new ImagePerceptualHash();
            _dtcFunction = new PerceptualDctHashFunction();
            _normalizedHammingDistance = new NormalizedHammingDistance();
        }
        /// <summary>
        /// 
        /// </summary>
        public void ScanDatabaseSystem()
        {
            var totalIndexed = DataBaseManager.GetTotalImagesWithoutPHash();

            var pagesCount = (totalIndexed / PageDataBaseSize) + 1;

            for (int i = 0; i < pagesCount; i++)
            {
                var results = DataBaseManager.GetImagesWithoutPHash(PageDataBaseSize);

                results.ForEach((documentImage) =>
                {
                    BitArray pHash;

                    if (documentImage.IsWithinFile)
                    {
                        pHash = GetImageHash(Path.Combine(TempFolder, documentImage.TempKeyName));
                    }
                    else
                    {
                        var file = DataBaseManager.GetFile(documentImage.FileId);
                        pHash = GetImageHash(GetDocumentFullPathName(file));
                    }

                    documentImage.PFingerPrint = Convert64BitArray(pHash);

                    DataBaseManager.UpdateDocumentImage(documentImage);

                    Console.WriteLine("Image {0} Hashed", documentImage.Id);
                });
            }
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="file"></param>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public IList<string> GetImageSimilarities(string file, int percentage)
        {
            var result1 = GetImageHash(file);

            var resultList = new List<string>();

            var totalIndexed = DataBaseManager.GetTotalCountImages();

            var pagesCount = (totalIndexed / PageDataBaseSize);

            if (pagesCount * PageDataBaseSize < totalIndexed) pagesCount++;

            for (var i = 0; i < pagesCount; i++)
            {
                //Get indexed Result
                var results = DataBaseManager.GetAllImagesPaged(i, PageDataBaseSize);

                results.AsParallel().ForAll(image =>
                {
                    var similarity = _normalizedHammingDistance.GetHashDistance(new BitArray(image.PFingerPrint), result1);
                    if (similarity > (percentage / (float)100))
                    {
                        var fileName = GetDocumentFullPathName(DataBaseManager.GetFile(image.FileId));
                        if (!resultList.Contains(fileName))
                        {
                            resultList.Add(fileName);    
                        }
                        
                    }
                }
                );
            }
            
            return resultList;
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
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private BitArray GetImageHash(string filePath)
        {
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var img = Image.FromStream(fs, true, false))
                    {
                        return _phash.GetDigest((Bitmap)img, _dtcFunction.GetHash);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[External] Image PHash Calculation Failed {0}:{1}", filePath, e.Message);
                return null;
            }
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public byte[] Convert64BitArray(BitArray bits)
        {
            if (bits == null || bits.Count != 64)
            {
                Console.WriteLine("Error getting Bit Array ");
                return null;
            }
            var newBytes = new byte[8];
            bits.CopyTo(newBytes, 0);
            return newBytes;
        }
    }


}
