//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FileDataAccess.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class DocumentFile
    {
        public DocumentFile()
        {
            this.DocumentImages = new HashSet<DocumentImage>();
        }
    
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string ItemType { get; set; }
        public string FolderPath { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public long Size { get; set; }
    
        public virtual ICollection<DocumentImage> DocumentImages { get; set; }
    }
}
