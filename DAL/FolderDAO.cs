using Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class FolderDAO
    {
        public static List<FolderDTO> GetAllFoldersByID(int id,int parent_id)
        { 
            try
            {
                var query = "Select * from dbo.Folder Where CreatedBy='" + id + "' AND ParentFolderId='"+ parent_id + "' AND IsActive=1 ";

                using (DBHelper helper = new DBHelper())
                {
                    var reader = helper.ExecuteReader(query);
                    List<FolderDTO> list = new List<FolderDTO>();

                    while (reader.Read())
                    {
                        var dto = FillDTO(reader);
                        if (dto != null)
                        {
                            list.Add(dto);
                        }
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        public static int CreateNewFolder(FolderDTO dto)
        {

            String sqlQuery = "";         
            sqlQuery = String.Format("INSERT INTO dbo.Folder(Name, ParentFolderId,CreatedBy,CreatedOn,IsActive) VALUES('{0}',{1},{2},'{3}',{4})",
                dto.Name,dto.ParentFolderID,dto.CreatedBy,dto.CreatedOn,1);
            try
            {
                using (DBHelper helper = new DBHelper())
                {
                    return helper.ExecuteQuery(sqlQuery);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }
        public static int DeleteFolder(int fid)
        {
            try
            {
                String sqlQuery = String.Format("Update dbo.Folder Set IsActive=0 Where ID={0}", fid);

                using (DBHelper helper = new DBHelper())
                {
                    return helper.ExecuteQuery(sqlQuery);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }
        public static FolderDTO GetFolderByID(int id)
        {
            try
            {
                var query = "Select * from dbo.Folder Where ID='" + id + "' AND IsActive=1 ";

                using (DBHelper helper = new DBHelper())
                {
                    var reader = helper.ExecuteReader(query);
                    FolderDTO folder= new FolderDTO();

                    while (reader.Read())
                    {
                        folder = FillDTO(reader);
                    }
                    return folder;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        public static List<FolderDTO> getMetaDataOfFolders(int id)
        {
            List<int> id_list = new List<int>();
            List<FolderDTO> folder_list = new List<FolderDTO>();
            int i = 0;
            id_list.Add(id);
            do
            {
                String name = "ROOT";
                String query = "Select * from dbo.Folder where ParentFolderId='" + id_list[i] + "'";
                try
                {
                    using (DBHelper helper = new DBHelper())
                    {
                        FolderDTO folder = new FolderDTO();
                        var result = helper.ExecuteReader(query);
                        while (result.Read())
                        {
                            using (DBHelper helper2 = new DBHelper())
                            {
                                String parent_query = "Select * from dbo.Folder where ID ='" + id_list[i] + "'";
                                var parent = helper2.ExecuteReader(parent_query);
                                if (parent.Read())
                                {
                                    name = parent.GetString(1);
                                }
                            }
                            folder = FillDTO(result);
                            folder.parentFolderName = name;
                            folder_list.Add(folder);
                            id_list.Add(result.GetInt32(0));
                        }
                    }
                    i++;
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }

            } while (i < id_list.Count);
            return folder_list;
        }
        private static FolderDTO FillDTO(SqlDataReader reader)
        {
            var dto = new FolderDTO();
            dto.ID = reader.GetInt32(0);
            dto.Name = reader.GetString(1);
            dto.ParentFolderID = reader.GetInt32(2);
            dto.CreatedBy = reader.GetInt32(3);
            dto.CreatedOn = reader.GetDateTime(4);
            dto.IsActive = reader.GetBoolean(5);
          

            return dto;
        }
    }
   
}
