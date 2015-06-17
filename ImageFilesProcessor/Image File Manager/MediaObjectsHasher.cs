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
        public enum HashMethod
        {
            BlockMeanMethod1UnOverlapped,
            BlockMeanMethod2Overlapped,
            BlockMeanMethod3UnOverlapped,
            BlockMeanMethod4Overlapped,
            DctMethod
        }
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
        private readonly BlockMeanHashFunction _blockMeanHashFunction;
        private readonly NormalizedHammingDistance _normalizedHammingDistance;
        /// <summary>
        /// 
        /// </summary>
        public MediaObjectsHasher()
        {
            _phash = new ImagePerceptualHash();
            _dtcFunction = new PerceptualDctHashFunction();
            _normalizedHammingDistance = new NormalizedHammingDistance();
            _blockMeanHashFunction = new BlockMeanHashFunction(BlockMeanHashFunction.MethodType.Method1UnOverlapped);
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


                    foreach (var method in Enum.GetValues(typeof(HashMethod)))
                    {
                        var hashFunction = (HashMethod) Enum.Parse(typeof (HashMethod), method.ToString());
                        
                        if (documentImage.IsWithinFile)
                        {
                            pHash = GetImageHash(Path.Combine(TempFolder, documentImage.TempKeyName),hashFunction );
                        }
                        else
                        {
                            var file = DataBaseManager.GetFile(documentImage.FileId);
                            pHash = GetImageHash(GetDocumentFullPathName(file),hashFunction);
                        }
                        AttachHashToDocumentImage(documentImage,pHash,hashFunction);
                    }

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
        public IList<string> GetImageSimilarities(string file, int percentage,HashMethod method)
        {
            var result1 = GetImageHash(file,method);

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
                    var similarity = _normalizedHammingDistance.GetHashDistance(new BitArray(GetHashType(image,method)), result1);
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
        /// <param name="method">Method used to hash Image</param>
        /// <returns></returns>
        private BitArray GetImageHash(string filePath,HashMethod method)
        {
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var img = Image.FromStream(fs, true, false))
                    {
                        switch (method)
                        {
                            case HashMethod.BlockMeanMethod1UnOverlapped:
                                _blockMeanHashFunction.ChangeMethod(BlockMeanHashFunction.MethodType.Method1UnOverlapped);
                                return _phash.GetHash((Bitmap)img, _blockMeanHashFunction.GetHash);
                            case HashMethod.BlockMeanMethod2Overlapped:
                                _blockMeanHashFunction.ChangeMethod(BlockMeanHashFunction.MethodType.Method2Overlapped);
                                return _phash.GetHash((Bitmap)img, _blockMeanHashFunction.GetHash);
                            case HashMethod.BlockMeanMethod3UnOverlapped:
                                _blockMeanHashFunction.ChangeMethod(BlockMeanHashFunction.MethodType.Method3UnOverlapped);
                                return _phash.GetHash((Bitmap)img, _blockMeanHashFunction.GetHash);
                            case HashMethod.BlockMeanMethod4Overlapped:
                                _blockMeanHashFunction.ChangeMethod(BlockMeanHashFunction.MethodType.Method4Overlapped);
                                return _phash.GetHash((Bitmap)img, _blockMeanHashFunction.GetHash);
                            case HashMethod.DctMethod:
                                return _phash.GetHash((Bitmap)img, _dtcFunction.GetHash);
                        }
                        return null;
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
        public byte[] ConvertToBitArray(BitArray bits)
        {
            if(bits.Length%8 != 0) throw new Exception("bits length must be 8 multiple");
            var newBytes = new byte[bits.Length/8];
            bits.CopyTo(newBytes, 0);
            return newBytes;
        }

        public void AttachHashToDocumentImage(DocumentImage documentImage,BitArray phash, HashMethod method)
        {
            var byteArray = ConvertToBitArray(phash);
            switch (method)
            {
                case HashMethod.BlockMeanMethod1UnOverlapped:
                    documentImage.BlockMeanHashM1 = byteArray;
                    break;
                case HashMethod.BlockMeanMethod2Overlapped:
                    documentImage.BlockMeanHashM2 = byteArray;
                    break;
                case HashMethod.BlockMeanMethod3UnOverlapped:
                    documentImage.BlockMeanHashM3 = byteArray;
                    break;
                case HashMethod.BlockMeanMethod4Overlapped:
                    documentImage.BlockMeanHashM4 = byteArray;
                    break;
                case HashMethod.DctMethod:
                    documentImage.DctHash = byteArray;
                    break;
            }
        }

        public Byte[] GetHashType(DocumentImage documentImage, HashMethod method)
        {
            switch (method)
            {
                case HashMethod.BlockMeanMethod1UnOverlapped:
                    return documentImage.BlockMeanHashM1;
                case HashMethod.BlockMeanMethod2Overlapped:
                    return documentImage.BlockMeanHashM2;
                case HashMethod.BlockMeanMethod3UnOverlapped:
                    return documentImage.BlockMeanHashM3;
                case HashMethod.BlockMeanMethod4Overlapped:
                    return documentImage.BlockMeanHashM4;
                case HashMethod.DctMethod:
                    return documentImage.DctHash;
            }
            return null;
        }
    }


}
