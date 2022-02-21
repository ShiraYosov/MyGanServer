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

        public const int UNPERMITTED_STATUS = 1;
        public const int PERMITTED_STATUS = 2;
       

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

        public List<PendingTeacher> GetTeachersList(int kID)
        {
            List<PendingTeacher> teachers = new List<PendingTeacher>();

            foreach (PendingTeacher teacher in this.PendingTeachers.Include(t => t.User).Include(g => g.Group))
            {
                if (teacher.StatusId == WAITING_STATUS && IsTeacherInKindergarten(kID, teacher))
                {
                    teachers.Add(teacher);
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
                this.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                SaveChanges();

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

              

                return true;



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool ChangeStatusForUser(object u)
        {
            try
            {
                if (u is StudentOfUser)
                {
                    StudentOfUser sou = (StudentOfUser)u;
                    StudentOfUser studentOfUser = new StudentOfUser();
                    studentOfUser = this.StudentOfUsers.Where(s => s.UserId == sou.UserId).FirstOrDefault();
                    studentOfUser.StatusId = sou.StatusId;
                    this.StudentOfUsers.Update(studentOfUser);
                    this.SaveChanges();

                    return true;
                }

                else if( u is PendingTeacher)
                {
                    PendingTeacher pTeacher = (PendingTeacher)u;
                    PendingTeacher teacher = new PendingTeacher();
                    teacher = this.PendingTeachers.Where(t => t.UserId == pTeacher.UserId).FirstOrDefault();
                    teacher.StatusId = pTeacher.StatusId;

                    if(teacher.StatusId == PERMITTED_STATUS)
                    {
                        Group gr = this.Groups.Where(g => g.GroupId == pTeacher.GroupId).FirstOrDefault();
                        User teacherUser = this.Users.Where(tu => tu.UserId == pTeacher.UserId).FirstOrDefault();
                        teacherUser.Groups.Add(gr);
                        gr.Teacher = teacherUser;
                        this.Groups.Update(gr);
                        this.Users.Update(teacherUser);
                    }

                    this.PendingTeachers.Update(teacher);
                    this.SaveChanges();

                    return true;
                }

                else { return false; }



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
