using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Entities;
using DAL;
namespace BAL
{
public class FolderBO
    {
        public static List<FolderDTO> GetAllFoldersByID(int id, int parent_id)
        {
           return FolderDAO.GetAllFoldersByID(id,parent_id);
        }
        public static FolderDTO GetFolderByID(int id)
        {
            return FolderDAO.GetFolderByID(id);
        }
        public static int CreateNewFolder(FolderDTO dto)
        {
            return FolderDAO.CreateNewFolder(dto);
        }
        public static int DeleteFolder(int fid)
        {
            return FolderDAO.DeleteFolder(fid);
        }
        public static int DeleteFile(int fid)
        {
            return FileDAO.DeleteFile(fid);

        }
        public static List<FolderDTO> getMetaDataOfFolders(int id)
        {
            return getMetaDataOfFolders(id);
        }

}
}
