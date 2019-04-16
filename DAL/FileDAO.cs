using DAL;
using Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class FileDAO
    {
        public static List<FileDTO> GetAllFilesByID(int id, int parent_id)
        {
            try
            {
                var query = "Select * from dbo.Files Where CreatedBy='" + id + "' AND ParentFolderId='" + parent_id + "' AND IsActive=1 ";

                using (DBHelper helper = new DBHelper())
                {
                    var reader = helper.ExecuteReader(query);
                    List<FileDTO> list = new List<FileDTO>();

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
        public static FileDTO GetFileByID(int id)
        {
            try
            {
                var query = "Select * from dbo.Files Where CreatedBy='" + id + "' AND IsActive=1 ";

                using (DBHelper helper = new DBHelper())
                {
                    var reader = helper.ExecuteReader(query);
                    FileDTO file = new FileDTO();

                    while (reader.Read())
                    {
                        var dto = FillDTO(reader);
                        
                    }
                    return file ;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        public static List<FileDTO> getMetaDataOfFiles(int id)
        {
            List<int> id_list = new List<int>();
            List<FileDTO> files_list = new List<FileDTO>();
            int i = 0;
            id_list.Add(id);
            do
            {
                String name = "ROOT";
                String query = "Select * from dbo.Files where ParentFolderId='" + id_list[i] + "'";
                try
                {
                    using (DBHelper helper = new DBHelper())
                    {
                        FileDTO file = new FileDTO();
                        var result = helper.ExecuteReader(query);
                        while (result.Read())
                        {
                            using (DBHelper helper2 = new DBHelper())
                            {
                                String parent_query = "Select * from dbo.Folder where Id ='" + id_list[i] + "'";
                                var parent = helper2.ExecuteReader(parent_query);
                                if (parent.Read())
                                {
                                    name = parent.GetString(1);
                                }
                            }
                            file = FillDTO(result);
                            file.parentFolderName = name;
                            files_list.Add(file);
                        }
                        using (DBHelper helper3 = new DBHelper())
                        {
                            String parentFol = "Select * from dbo.Folder where ParentFolderId='" + id_list[i] + "'";
                            var fid_result = helper3.ExecuteReader(parentFol);
                            while (fid_result.Read())
                            {
                                id_list.Add(fid_result.GetInt32(0));
                            }
                        }

                    }
                    i++;
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }

            } while (i < id_list.Count);
            return files_list;
        }
        public static FileDTO GetFileByFileID(int id)
        {
            try
            {
                var query = "Select * from dbo.Files Where ID='" + id + "' ";

                using (DBHelper helper = new DBHelper())
                {
                    var reader = helper.ExecuteReader(query);
                    FileDTO file = new FileDTO();

                    while (reader.Read())
                    {
                        file = FillDTO(reader);

                    }
                    return file;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        public static FileDTO GetFileByName(String name)
        {
            try
            {
                var query = "Select * from dbo.Files Where Name='" + name + "' AND IsActive=1 ";

                using (DBHelper helper = new DBHelper())
                {
                    var reader = helper.ExecuteReader(query);
                    FileDTO file = new FileDTO();

                    while (reader.Read())
                    {
                        var dto = FillDTO(reader);

                    }
                    return file;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        public static int DeleteFile(int fid)
        {
            try
            {
                String sqlQuery = String.Format("Update dbo.Files Set IsActive=0 Where ID={0}", fid);

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
        public static int saveFileInDB(FileDTO dto)
        {
            String query = String.Format("INSERT INTO dbo.Files(Name,ParentFolderId,FileExt,FileSizeInKB,CreatedBy,UploadedOn,IsActive,ContentType,fileUniqueName) VALUES('{0}',{1},'{2}',{3},{4},'{5}',{6},'{7}','{8}')",
                dto.Name, dto.ParentFolderID, dto.FileExt, dto.FileSizeInKB, dto.CreatedBy, dto.UploadedOn, 1,dto.ContentType,dto.fileUniqueName);
            try
            {
                using (DBHelper helper = new DBHelper())
                {
                    return helper.ExecuteQuery(query);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }
        private static FileDTO FillDTO(SqlDataReader reader)
        {
            var dto = new FileDTO();
            dto.ID = reader.GetInt32(0);
            dto.Name = reader.GetString(1);
            dto.ParentFolderID = reader.GetInt32(2);
            dto.FileExt = reader.GetString(3);
            dto.FileSizeInKB = reader.GetInt32(4);
            dto.CreatedBy = reader.GetInt32(5);
            dto.UploadedOn = reader.GetDateTime(6);
            dto.IsActive = reader.GetBoolean(7);
            dto.ContentType = reader.GetString(8);
            dto.fileUniqueName = reader.GetString(9);
            return dto;
        }
    }
}
