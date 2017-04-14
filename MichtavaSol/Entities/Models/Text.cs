using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entities.Models
{
    public class Text : DeletableEntity
    {
        //public enum FileFormats { doc, docx, pdf, txt };


        public string Id { get; set; }
        
        public string Name { get; set; }

        public DateTime UploadTime { get; set; }

        //public FileFormats Format { get; set; }

        public string FilePath { get; set; }

        public Subject Subject { get; set; }
    }
}