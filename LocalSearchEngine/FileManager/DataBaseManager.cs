using System;
using System.Collections.Generic;
using System.Linq;
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
       
    }
}
