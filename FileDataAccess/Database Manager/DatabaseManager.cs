using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileDataAccess.Database;

namespace FileDataAccess
{
    /// <summary>
    /// Database Manager Keep all database operations logic
    /// </summary>
    public static class DataBaseManager
    {
        public static void ClearDataBase()
        {
            try
            {
                using (var context = new FileDataBaseContainer())
                {
                    context.Database.ExecuteSqlCommand("DELETE FROM DocumentFiles WHERE ID != '"+Guid.Empty+"'");
                }
            }
            catch (Exception)
            {
                //TODO LOG EXCEPTION DETAILS 
            }

        }
        /// <summary>
        /// Gets the specific file document from a GUID
        /// </summary>
        /// <param name="fileId">GUID of Document File</param>
        /// <returns></returns>
        public static DocumentFile GetFile(Guid fileId)
        {
            try
            {
                using (var context = new FileDataBaseContainer())
                {
                    return context.DocumentFiles.First(file => file.Id == fileId);
                }
            }
            catch (Exception)
            {
                //TODO LOG EXCEPTION DETAILS 
                return null;
            }

        }
        /// <summary>
        /// Check if a file is already indexed in Database
        /// </summary>
        /// <param name="file">File to verify indexation</param>
        /// <returns>Is file indexed</returns>
        public static bool IsFileAlreadyIndexed(FileInfo file)
        {
            try
            {
                using (var context = new FileDataBaseContainer())
                {
                    return context.DocumentFiles.FirstOrDefault(e => e.FolderPath + "\\" + e.Name == file.FullName) != null;
                }
            }
            catch (Exception)
            {
                //TODO LOG EXCEPTION DETAILS 
                return false;
            }
        }

        public static DocumentFile GetFileByFullName(string fullName)
        {
            try
            {
                using (var context = new FileDataBaseContainer())
                {
                    return context.DocumentFiles.FirstOrDefault(e => e.FolderPath + "\\" + e.Name == fullName);
                }
            }
            catch (Exception)
            {
                //TODO LOG EXCEPTION DETAILS 
                return null;
            }
        }

        public static void UpdateFile(DocumentFile file)
        {
            try
            {
                using (var context = new FileDataBaseContainer())
                {
                    var result = context.DocumentFiles.First(e => e.Id == file.Id);
                    result.Name = file.Name;
                    result.FolderPath = file.FolderPath;
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                //TODO LOG EXCEPTION DETAILS 
                return;
            }
        }

        public static void UpdateDocumentImage(DocumentImage image)
        {
            try
            {
                using (var context = new FileDataBaseContainer())
                {
                    var result = context.DocumentImages.First(e => e.Id == image.Id);
                    result.DctHash = image.DctHash;
                    result.BlockMeanHashM1 = image.BlockMeanHashM1;
                    result.BlockMeanHashM2 = image.BlockMeanHashM2;
                    result.BlockMeanHashM3 = image.BlockMeanHashM3;
                    result.BlockMeanHashM4 = image.BlockMeanHashM4;
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                //TODO LOG EXCEPTION DETAILS 
                return;
            }
        }

        public static void SaveFile(DocumentFile file)
        {
            try
            {
                using (var context = new FileDataBaseContainer())
                {
                    context.DocumentFiles.Add(file);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                //TODO LOG EXCEPTION DETAILS 
            }
        }

        public static List<DocumentImage> GetFileImages(DocumentFile file)
        {
            try
            {
                using (var context = new FileDataBaseContainer())
                {
                    return (from p in context.DocumentImages
                            where p.FileId == file.Id
                            select p).ToList();
                }
            }
            catch (Exception)
            {
                //TODO LOG EXCEPTION DETAILS 
                return null;
            }
        }

        public static List<DocumentImage> GetFileInternalImages(DocumentFile file)
        {
            try
            {
                using (var context = new FileDataBaseContainer())
                {
                    return (from p in context.DocumentImages
                            where p.FileId == file.Id &&
                            p.IsWithinFile
                            select p).ToList();
                }
            }
            catch (Exception)
            {
                //TODO LOG EXCEPTION DETAILS 
                return null;
            }
        }


        public static int GetTotalFilesIndexed()
        {
            try
            {
                using (var context = new FileDataBaseContainer())
                {
                    return (from p in context.DocumentFiles select p).Count();
                }
            }
            catch (Exception)
            {
                //TODO LOG EXCEPTION DETAILS 
                return -1;
            }
        }

        public static List<DocumentFile> GetIndexedFilesPaged(int page, int pagesize)
        {
            try
            {
                using (var context = new FileDataBaseContainer())
                {
                    return (from p in context.DocumentFiles
                            orderby p.CreatedDate
                            select p).Skip(page * pagesize).Take(pagesize).ToList();
                }
            }
            catch (Exception)
            {
                //TODO LOG EXCEPTION DETAILS 
                return null;
            }
        }

        public static void DeleteDocumentFile(DocumentFile file)
        {
            try
            {
                using (var context = new FileDataBaseContainer())
                {
                    context.DocumentFiles.Remove(context.DocumentFiles.First(e => e.Id == file.Id));
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                //TODO LOG EXCEPTION DETAILS 
            }
        }

        public static int GetTotalImagesWithoutPHash()
        {
            try
            {
                using (var context = new FileDataBaseContainer())
                {
                    return (from p in context.DocumentImages where p.DctHash == null select p).Count();
                }
            }
            catch (Exception e)
            {
                //TODO LOG EXCEPTION DETAILS 
                return -1;
            }
        }

        public static int GetTotalCountImages()
        {
            try
            {
                using (var context = new FileDataBaseContainer())
                {
                    return (from p in context.DocumentImages select p).Count();
                }
            }
            catch (Exception e)
            {
                //TODO LOG EXCEPTION DETAILS 
                return -1;
            }
        }

        public static List<DocumentImage> GetImagesWithoutPHash(int batchSize)
        {
            try
            {
                using (var context = new FileDataBaseContainer())
                {
                    return (from p in context.DocumentImages
                            where p.DctHash == null
                            select p).Take(batchSize).ToList();
                }
            }
            catch (Exception)
            {
                //TODO LOG EXCEPTION DETAILS 
                return null;
            }
        }

        public static List<DocumentImage> GetAllImagesPaged(int page, int pagesize)
        {
            try
            {
                using (var context = new FileDataBaseContainer())
                {
                    return (from p in context.DocumentImages
                            orderby p.Id
                            select p).Skip(page * pagesize).Take(pagesize).ToList();
                }
            }
            catch (Exception)
            {
                //TODO LOG EXCEPTION DETAILS 
                return null;
            }
        }


    }
}
