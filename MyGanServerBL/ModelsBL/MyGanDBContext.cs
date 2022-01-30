using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using MyGanServerBL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MyGanServerBL.Models
{
    partial class MyGanDBContext
    {
        public User Login(string email, string pswd)
        {
            User user = new User();
            user = this.Users.
                      Include(g => g.Groups).
                      Include(sou => sou.StudentOfUsers). 
                      ThenInclude(st => st.Student).
                      Include(km => km.KindergartenManagers).
                      ThenInclude(k => k.Kindergarten).
                      Where(u => u.Email == email && u.Password == pswd).FirstOrDefault();
            return user;
        }

        public bool AddAllergy(Allergy allergy)
        {
            try
            {
                this.Allergies.Add(allergy);
                this.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool AddUser(User user)
        {
            try
            {
                this.Users.Add(user);
                this.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool TeacherRegister(User user)
        {
            try
            {
                Group userGroup = user.Groups.First();
                user.Groups.Clear();
                Group g = Groups.Where(g => g.GroupId == userGroup.GroupId).FirstOrDefault();
                g.Teacher = user;
                this.Groups.Update(g);
                this.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool ParentRegister(User user)
        {
            try
            {
                StudentOfUser studentOfUsers = user.StudentOfUsers.First();
                Student student = studentOfUsers.Student;
                this.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                this.Entry(studentOfUsers).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                this.Entry(student).State = Microsoft.EntityFrameworkCore.EntityState.Added;


                this.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }




    }
}
