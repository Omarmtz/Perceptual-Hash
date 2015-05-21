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
                    result.PFingerPrint = image.PFingerPrint;
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
                    return (from p in context.DocumentImages where p.PFingerPrint == null select p).Count();
                }
            }
            catch (Exception e)
            {
                //TODO LOG EXCEPTION DETAILS 
                return -1;
            }
        }

        public static List<DocumentImage> GetImagesWithoutPHashPaged(int page, int pagesize)
        {
            try
            {
                using (var context = new FileDataBaseContainer())
                {
                    return (from p in context.DocumentImages
                            where p.PFingerPrint == null
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
