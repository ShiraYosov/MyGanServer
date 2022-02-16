using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGanServerBL.Models;
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
                      ThenInclude(s => s.Students).
                      ThenInclude(sou => sou.StudentOfUsers).
                      ThenInclude(u => u.User).
                      Include(km => km.KindergartenManagers).
                      ThenInclude(k => k.Kindergarten).
                      ThenInclude(g => g.Groups).
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

        public bool StudentExist(string ID)
        {
            foreach (Student s in this.Students)
            {
                if (s.StudentId == ID)
                    return true;
            }
            return false;
        }

        public const int WAITING_STATUS = 3;

        public List<User> GetTeachersList(int kID)
        {
            List<User> teachers = new List<User>();

            foreach (PendingTeacher teacher in this.PendingTeachers)
            {
                if (teacher.StatusId == WAITING_STATUS && IsTeacherInKindergarten(kID, teacher))
                {
                    teachers.Add(teacher.User);
                }
            }
            return teachers;
        }

        public bool IsTeacherInKindergarten(int kID, PendingTeacher teacher)
        {
            if (teacher.Group.KindergartenId == kID)
                return true;
        
            return false;
        }

    public bool TeacherRegister(User user)
    {
        try
        {
            bool added = AddUser(user);

            if (added)
            {
                Group userGroup = user.Groups.First();
                user.Groups.Clear();
                Group g = Groups.Where(g => g.GroupId == userGroup.GroupId).FirstOrDefault();

                PendingTeacher newTeacher = new PendingTeacher
                {
                    UserId = user.UserId,
                    GroupId = g.GroupId,
                    StatusId = WAITING_STATUS
                };

                this.PendingTeachers.Add(newTeacher);
                this.SaveChanges();

                //g.Teacher = user;
                //this.Groups.Update(g);

                return true;
            }

            return false;

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public bool ChangeStatusForUser(int statusID, User u)
    {
        try
        {
            User user = new User();
            user = this.Users.Where(us => us.UserId == u.UserId).FirstOrDefault();
            //user.StatusId = statusID;
            this.Users.Update(user);
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

            if (StudentExist(student.StudentId))
            {
                this.Entry(student).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            else
            {
                this.Entry(student).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            }

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
