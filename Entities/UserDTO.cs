using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class UserDTO
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public String Login { get; set; }
        public String Password { get; set; }
        public String Email { get; set; }
    }
}
