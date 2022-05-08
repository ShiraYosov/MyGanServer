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

                      Include(sou => sou.StudentOfUsers).
                      ThenInclude(s => s.Student).
                      ThenInclude(g => g.Group).
                      ThenInclude(e => e.Events).
                      ThenInclude(p => p.Photos).

                      Include(g => g.Groups).
                      ThenInclude(s => s.Students).
                      ThenInclude(sou => sou.StudentOfUsers).
                      ThenInclude(r => r.RelationToStudent).

                      Include(k => k.KindergartenManagers).
                      ThenInclude(ki => ki.Kindergarten).
                      ThenInclude(g => g.Groups).
                      ThenInclude(e => e.Events).
                      ThenInclude(p => p.Photos).
                      ThenInclude(e => e.Event).

                      Include(k => k.KindergartenManagers).
                      ThenInclude(ki => ki.Kindergarten).
                      ThenInclude(g => g.Groups).
                      ThenInclude(m => m.Messages).
                      ThenInclude(g => g.Group).
                      ThenInclude(ms => ms.Messages).

                      Include(g => g.Groups).
                      ThenInclude(e => e.Events).
                      ThenInclude(p => p.Photos).
                      ThenInclude(e => e.Event).

                      Include(g => g.Groups).
                      ThenInclude(s => s.Students).
                      ThenInclude(g => g.Grade).

                      Include(s => s.StudentOfUsers).
                      ThenInclude(st => st.Student).
                      ThenInclude(g => g.Group).
                      ThenInclude(m => m.Messages).
                      ThenInclude(u => u.User).

                      Include(st => st.StudentOfUsers).
                      ThenInclude(s => s.Student).
                      ThenInclude(sa => sa.StudentAllergies).
                      ThenInclude(a => a.Allergy).

                      Include(km => km.KindergartenManagers).
                      ThenInclude(k => k.Kindergarten).
                      ThenInclude(g => g.Groups).
                      ThenInclude(t => t.Teacher).

                      Include(sou => sou.StudentOfUsers).
                      ThenInclude(s => s.Student).
                      ThenInclude(g => g.Grade).

                      Include(g => g.Groups).
                      ThenInclude(m => m.Messages).
                      ThenInclude(u => u.User).

                      Include(g => g.Groups).
                      ThenInclude(s =>s.Students).
                      ThenInclude(sa => sa.StudentAllergies).
                      ThenInclude(a => a.Allergy).

                      Include(g => g.Groups).
                      ThenInclude(m => m.Messages).
                      ThenInclude(g => g.Group).
                      ThenInclude(ms => ms.Messages).

                      Include(km => km.KindergartenManagers).
                      ThenInclude(k => k.Kindergarten).
                      ThenInclude(g => g.Groups).
                      ThenInclude(s => s.Students).
                      ThenInclude(u => u.StudentOfUsers).
                      ThenInclude(u => u.User).
                      ThenInclude(su => su.StudentOfUsers).
                      ThenInclude(s => s.Student).
                      ThenInclude(g => g.Grade).

                      Include(km => km.KindergartenManagers).
                      ThenInclude(k => k.Kindergarten).
                      ThenInclude(g => g.Groups).
                      ThenInclude(s => s.Students).
                      ThenInclude(u => u.StudentOfUsers).
                      ThenInclude(r => r.RelationToStudent).

                      Include(km => km.KindergartenManagers).
                      ThenInclude(k => k.Kindergarten).
                      ThenInclude(g => g.Groups).
                      ThenInclude(s => s.Students).
                      ThenInclude(a => a.StudentAllergies).
                      ThenInclude(al => al.Allergy)


                      .Where(u => u.Email == email && u.Password == pswd).FirstOrDefault();


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

        public Photo GetPhoto(int photoID)
        {
            Photo photo = this.Photos.Include(e => e.Event).Include(u => u.User).Where(p => p.Id == photoID).FirstOrDefault();
            return photo;
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

                else if (u is PendingTeacher)
                {
                    PendingTeacher pTeacher = (PendingTeacher)u;
                    PendingTeacher teacher = new PendingTeacher();
                    teacher = this.PendingTeachers.Where(t => t.UserId == pTeacher.UserId).FirstOrDefault();
                    teacher.StatusId = pTeacher.StatusId;

                    if (teacher.StatusId == PERMITTED_STATUS)
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

        public bool ParentRegister(User user, Student student)
        {
            try
            {

                this.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Added;

                this.SaveChanges();

                if (!StudentExist(student.StudentId))
                {
                    this.ChangeTracker.Clear();
                    this.Entry(student).State = Microsoft.EntityFrameworkCore.EntityState.Added;

                    foreach (StudentAllergy allergy in student.StudentAllergies)
                    {
                        allergy.AllergyId = allergy.Allergy.AllergyId;
                        this.Entry(allergy).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    }

                    StudentOfUser st = user.StudentOfUsers.Where(s => s.Student.StudentId == student.StudentId).FirstOrDefault();
                    StudentOfUser stu = new StudentOfUser()
                    {
                        StudentId = student.StudentId,
                        UserId = user.UserId,
                        RelationToStudent = st.RelationToStudent,
                        StatusId = st.StatusId

                    };

                    this.Entry(stu).State = Microsoft.EntityFrameworkCore.EntityState.Added;

                    this.SaveChanges();
                }



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
