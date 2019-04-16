using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class FileDTO
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public int ParentFolderID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UploadedOn { get; set; }
        public Boolean IsActive { get; set; }
        public String fileUniqueName { get; set; }
        public String FileExt { get; set; }
        public int FileSizeInKB { get; set; }
        public String ContentType { get; set; }
        public String parentFolderName { get; set; }

    }
}
