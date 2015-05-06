using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LocalSearchEngine.DataAccess;

namespace LocalSearchEngine.FileManager
{
    public static class DataBaseManager
    {
        public static DocumentFile GetFile(Guid fileId)
        {
            try
            {
                using (var context = new FileManagementDBContainer())
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

        public static bool IsFileAlreadyIndexed(FileInfo file)
        {
            try
            {
                using (var context = new FileManagementDBContainer())
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
                using (var context = new FileManagementDBContainer())
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
                using (var context = new FileManagementDBContainer())
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

        public static void SaveFile(DocumentFile file)
        {
            try
            {
                using (var context = new FileManagementDBContainer())
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
                using (var context = new FileManagementDBContainer())
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
                using (var context = new FileManagementDBContainer())
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
                using (var context = new FileManagementDBContainer())
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
                using (var context = new FileManagementDBContainer())
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
                using (var context = new FileManagementDBContainer())
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

    }
}
