using DAL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
   public  class FileBO
    {
        public static List<FileDTO> GetAllFilesByID(int id, int parent_id)
        {
            return FileDAO.GetAllFilesByID(id, parent_id);
        }
        public static FileDTO GetFileByID(int id)
        {
            return FileDAO.GetFileByID(id);
        }
        public static FileDTO GetFileByName(String name)
        {
            return FileDAO.GetFileByName(name);
        }
        public static FileDTO GetFileByFileID(int id)
        {
            return FileDAO.GetFileByFileID(id);
        }
        public static int DeleteFile(int fid)
        {
            return FileDAO.DeleteFile(fid);
        }
        public static int saveFileInDB(FileDTO fileDTO)
        {
            return FileDAO.saveFileInDB(fileDTO);
        }
        public static List<FileDTO> getMetaDataOfFiles(int id)
        {
            return getMetaDataOfFiles(id);

        }
}
}
