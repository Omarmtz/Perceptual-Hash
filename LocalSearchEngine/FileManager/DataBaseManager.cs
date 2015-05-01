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
        public static File GetFile(Guid fileId)
        {
            try
            {
                using (var context = new FileManagementDBContainer())
                {
                    return context.Files.First(file => file.Id == fileId);
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
