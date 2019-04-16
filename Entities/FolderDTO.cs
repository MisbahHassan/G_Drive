using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
   public  class FolderDTO
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public int ParentFolderID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Boolean IsActive { get; set; }
        public String parentFolderName { get; set; }

    }
}
