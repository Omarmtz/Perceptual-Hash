//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LocalSearchEngine.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Image
    {
        public System.Guid Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public short Bpp { get; set; }
        public byte[] PFingerPrint { get; set; }
        public System.Guid FileId { get; set; }
    
        public virtual File File { get; set; }
    }
}
