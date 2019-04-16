using Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class UserDAO
    {
        public static UserDTO ValidateUser(String pLogin, String pPassword)
        {
            try
            {
                var query = String.Format("Select * from dbo.Users Where Login='{0}' and Password='{1}'", pLogin, pPassword);

                using (DBHelper helper = new DBHelper())
                {
                    var reader = helper.ExecuteReader(query);

                    UserDTO dto = null;

                    if (reader.Read())
                    {
                        dto = FillDTO(reader);
                    }

                    return dto;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        public static UserDTO getUserByLogin(String login)
        {
            try
            {
                var query = String.Format("Select * from dbo.Users Where Login ='" + login+ "' ");
                using (DBHelper helper = new DBHelper())
                {
                    var reader = helper.ExecuteReader(query);

                    UserDTO dto = null;

                    if (reader.Read())
                    {
                        dto = FillDTO(reader);
                    }

                    return dto;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        private static UserDTO FillDTO(SqlDataReader reader)
        {
            var dto = new UserDTO();
            dto.ID = reader.GetInt32(0);
            dto.Name = reader.GetString(1);
            dto.Login = reader.GetString(2);
            dto.Password = reader.GetString(3);
            dto.Email = reader.GetString(4);

            return dto;
        }
    }
    
}
