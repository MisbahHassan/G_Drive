using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class UserBO
    {

        public static UserDTO ValidateUser(String pLogin, String pPassword)
        {
            return DAL.UserDAO.ValidateUser(pLogin, pPassword);
        }
        public static UserDTO getUserByLogin(String login)
        {
            return DAL.UserDAO.getUserByLogin(login);
        }
    }
}
