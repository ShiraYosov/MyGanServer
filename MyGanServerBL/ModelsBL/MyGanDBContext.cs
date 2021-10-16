using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGanServerBL.Models
{
    partial class MyGanDBContext
    {
        public User Login(string email, string pswd)
        {
            User user = this.Users.Where(u => u.Email == email && u.Password == pswd).FirstOrDefault();

            return user;
        }
    }
}
