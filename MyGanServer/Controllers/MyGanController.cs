using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyGanServerBL.Models;
using MyGanServer.DTO;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using MyGanServer.Helper;

namespace MyGanServer.Controllers
{
    [Route("MyGanAPI")]
    [ApiController]
    public class MyGanController : ControllerBase
    {
        #region Add connection to the db context using dependency injection
        MyGanDBContext context;
        public MyGanController(MyGanDBContext context)
        {
            this.context = context;
        }
        #endregion


        //Create random string
        public static string GenerateAlphanumerical(int size)
        {
            char[] chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[4 * size];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }
            return result.ToString();
        }

        public const int PERMITTED_STATUS = 2;

        [Route("Login")]
        [HttpGet]
        public User Login([FromQuery] string email, [FromQuery] string pass)
        {
            try
            {


                User user = context.Login(email, pass);

                //Check user name and password
                if (user != null)
                {
                    HttpContext.Session.SetObject("theUser", user);

                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

                    return user;
                }
                else
                {

                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        [Route("GetLookups")]
        [HttpGet]
        public Lookups GetLookups()
        {
            try
            {
                Lookups obj = new Lookups()
                {
                    Grades = context.Grades.ToList(),
                    Allergies = context.Allergies.ToList(),
                    Relations = context.RelationToStudents.ToList()
                };
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return obj;
            }

            catch
            {
                return null;
            }
        }

        [Route("CodeExist")]
        [HttpGet]
        public bool CodeExist([FromBody] int code)
        {
            try
            {
                foreach (Group g in context.Groups)
                {
                    if (g.GroupId == code) { return true; }
                }
                return false;
            }

            catch
            {
                return false;
            }
        }

        [Route("GetTeachersWithWaitStatus")]
        [HttpGet]
        public List<PendingTeacher> GetTeachersWithWaitStatus([FromQuery] int kindergartenID)
        {
            try
            {


                User user = HttpContext.Session.GetObject<User>("theUser");

                if (user != null)
                {
                    List<PendingTeacher> teachersList = this.context.GetTeachersList(kindergartenID);
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

                    return teachersList;
                }

                else
                {

                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return null;
                }
            }
            catch
            {
                return null;
            }


        }


        [Route("ChangeTeacherStatus")]
        [HttpPost]
        public bool ChangeTeacherStatus(PendingTeacher t)
        {
            try
            {


                User user = HttpContext.Session.GetObject<User>("theUser");

                if (user != null)
                {

                    bool ok = this.context.ChangeStatusForUser(t);

                    if (ok)
                    {
                        //Check if user is permitted
                        if (t.StatusId == PERMITTED_STATUS)
                        {
                            EmailSender.SendEmail2("עדכון", $"  בקשת ההרשמה שלך אושרה! מהר/י להתחבר  ", $"{t.User.Email}", $"{t.User.Fname} {t.User.LastName}", "<ganenu1@gmail.com>", $"גננו", "#GANENU123!", "smtp.gmail.com");
                        }

                        Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                        return true;
                    }
                    else
                    {
                        Response.StatusCode = (int)System.Net.HttpStatusCode.NotModified;
                        return false;
                    }
                }


                else
                {

                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return false;
                }
            }
            catch
            {
                return false;
            }


        }

        [Route("ChangeParentStatus")]
        [HttpPost]
        public bool ChangeParentStatus(StudentOfUser s)
        {
            try
            {


                User user = HttpContext.Session.GetObject<User>("theUser");

                if (user != null)
                {

                    bool ok = this.context.ChangeStatusForUser(s);

                    if (ok)
                    {
                        //Check if user is permitted
                        if (s.StatusId == PERMITTED_STATUS)
                        {
                            EmailSender.SendEmail2("עדכון", $"  בקשת ההרשמה שלך אושרה! מהר/י להתחבר  ", $"{s.User.Email}", $"{s.User.Fname} {s.User.LastName}", "<ganenu1@gmail.com>", $"גננו", "#GANENU123!", "smtp.gmail.com");
                        }

                        Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                        return true;
                    }
                    else
                    {
                        Response.StatusCode = (int)System.Net.HttpStatusCode.NotModified;
                        return false;
                    }


                }

                else
                {

                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return false;
                }
            }
            catch
            {
                return false;
            }


        }


        [Route("UploadImage")]
        [HttpPost]

        public async Task<IActionResult> UploadImage(IFormFile file)
        {
           
            User user = HttpContext.Session.GetObject<User>("theUser");
            //Check if user logged in and its ID is the same as the contact user ID
            if (user != null)
            {
                if (file == null)
                {
                    return BadRequest();
                }

                try
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }


                    return Ok(new { length = file.Length, name = file.FileName });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return BadRequest();
                }
            }
            return Forbid();
        }

        [Route("AddGroup")]
        [HttpPost]

        public Group AddGroup([FromBody] Group group)
        {
            try
            {


                User user = HttpContext.Session.GetObject<User>("theUser");

                if (user != null)
                {
                    //Check if group is not contained in the groups list
                    if (!context.Groups.Contains(group))
                    {
                        context.Entry(group).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                        context.SaveChanges();

                    }
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return group;
                }
                else
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return null;
                }
            }
            catch
            {
                return null;
            }

        }

        [Route("AddEvent")]
        [HttpPost]

        public Event AddEvent([FromBody] Event ev)
        {
           
                User user = HttpContext.Session.GetObject<User>("theUser");

                if (user != null)
                {
                    try
                    {
                        context.Entry(ev).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                        context.SaveChanges();

                        Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                        return ev;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return null;
                    }

                }
                else
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return null;
                }
         

        }

        [Route("AddPhoto")]
        [HttpPost]

        public Photo AddPhoto([FromBody] Photo photo)
        {
            User user = HttpContext.Session.GetObject<User>("theUser");

            if (user != null)
            {
                try
                {
                    context.Entry(photo).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    context.SaveChanges();

                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return photo;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }

            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }

        }

        [Route("EditPhotoDescription")]
        [HttpPost]

        public bool EditDescription([FromBody] Photo photo)
        {
            User user = HttpContext.Session.GetObject<User>("theUser");

            if (user != null)
            {
                try
                {
                    Photo curr = context.Photos.Where(p => p.Id == photo.Id).FirstOrDefault();
                    curr.Description = photo.Description;
                    context.Entry(curr).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();

                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }

            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return false;
            }

        }

        [Route("SendMessage")]
        [HttpPost]

        public Message SendMessage([FromBody] Message message)
        {
            User user = HttpContext.Session.GetObject<User>("theUser");

            if (user != null)
            {
                try
                {


                    context.Entry(message).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    foreach (Student st in message.Group.Students)
                    {
                        //Send email message
                        foreach (StudentOfUser u in st.StudentOfUsers)
                        {
                            EmailSender.SendEmail2("הודעה חדשה!", $"  {message.Content} -התקבלה הודעה חדשה  ", $"{u.User.Email}", $"{u.User.Fname} {u.User.LastName}", "<ganenu1@gmail.com>", $"גננו", "#GANENU123!", "smtp.gmail.com");
                        }
                    }
                    context.SaveChanges();


                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return message;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }

        }

        [Route("DeleteMessage")]
        [HttpPost]

        public bool DeleteMessage([FromBody] Message message)
        {
            User user = HttpContext.Session.GetObject<User>("theUser");

            if (user != null)
            {
                try
                {


                    Message toDelete = context.Messages.Where(m => m.MessageId == message.MessageId).FirstOrDefault();
                    context.Entry(toDelete).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

                    context.SaveChanges();


                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return false;
            }

        }

        [Route("DeleteEvent")]
        [HttpPost]

        public bool DeleteEvent([FromBody] Event ev)
        {
            User user = HttpContext.Session.GetObject<User>("theUser");

            if (user != null)
            {
                try
                {


                    Event toDelete = context.Events.Where(e => e.EventId == ev.EventId).FirstOrDefault();
                    //Delete all photos from event
                    foreach (Photo p in ev.Photos)
                    {
                        DeletePhoto(p.Id);
                    }
                    context.SaveChanges();

                    context.Entry(toDelete).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    context.SaveChanges();


                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return false;
            }

        }

        [Route("DeletePhoto")]
        [HttpPost]

        public bool DeletePhoto([FromBody] int photoID)
        {
            User user = HttpContext.Session.GetObject<User>("theUser");

            if (user != null)
            {
                try
                {


                    Photo toDelete = context.GetPhoto(photoID);
                    //Delet photo file
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Events", toDelete.Id + ".jpg");
                    System.IO.File.Delete(path);
                    context.Entry(toDelete).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    context.Entry(toDelete.Event).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.Entry(toDelete.User).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                    context.SaveChanges();


                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return false;
            }

        }

        [Route("AddManager")]
        [HttpPost]

        public bool AddManager([FromBody] KindergartenManager kManager)
        {
            User user = HttpContext.Session.GetObject<User>("theUser");

            if (user != null)
            {
                try
                {


                    if (!context.KindergartenManagers.Contains(kManager))
                    {
                        context.Entry(kManager).State = Microsoft.EntityFrameworkCore.EntityState.Added;

                        context.SaveChanges();

                    }
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return false;
            }

        }


        [Route("AddAllergy")]
        [HttpPost]
        public bool AddAllergy([FromBody] Allergy allergy)
        {

            if (allergy != null)
            {
                try
                {


                    bool added = this.context.AddAllergy(allergy);
                    if (added)
                    {
                        Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                        return added;
                    }
                    else
                        Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return false;
            }


        }

        [Route("Logout")]
        [HttpPost]
        public bool Logout([FromBody] Object user)
        {
            try
            {
                Object current = HttpContext.Session.GetObject<Object>("theUser");

                if (current != null)
                {
                    HttpContext.Session.Remove("theUser");
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return true;
                }
                else
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return false;
                }
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return false;
            }
        }

        [Route("Register")]
        [HttpPost]
        public User Register([FromBody] User user)
        {
            //If user is null the request is bad
            if (user == null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return null;
            }

            User currentUser = HttpContext.Session.GetObject<User>("theUser");
            //Check if user logged in and its ID is the same as the contact user ID
            if (currentUser != null && currentUser.UserId == user.UserId)
            {
                //Check if user is new or if it already exists
                if (user.UserId > 0)
                {
                    context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    foreach (KindergartenManager km in user.KindergartenManagers)
                    {
                        Kindergarten k = km.Kindergarten;
                        context.Entry(k).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    }

                    HttpContext.Session.SetObject("theUser", user);
                    context.SaveChanges();

                }

                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return user;

            }

            else if (user != null)
            {
                //Create new user password 
                if (string.IsNullOrEmpty(user.Password))
                {
                    user.Password = GenerateAlphanumerical(6);
                    context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    context.Entry(user.KindergartenManagers.Last()).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    context.SaveChanges();

                    //send email
                    EmailSender.SendEmail2("ברוכים הבאים!", $"  {user.Password} - סיסמתך לאפליקציה  ", $"{user.Email}", $"{user.Fname} {user.LastName}", "<ganenu1@gmail.com>", $"גננו", "#GANENU123!", "smtp.gmail.com");
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return user;
                }
                context.AddUser(user);
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return user;
            }

            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }
        }







        [Route("TeacherRegister")]
        [HttpPost]
        public User TeacherRegister([FromBody] User user)
        {
            //If user is null the request is bad
            if (user == null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return null;
            }

            User currentUser = HttpContext.Session.GetObject<User>("theUser");
            //Check if user logged in and its ID is the same as the contact user ID
            if (currentUser != null && currentUser.UserId == user.UserId)
            {
                //Check if user is new or if it already exists
                if (user.UserId > 0)
                {
                    context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                    foreach (PendingTeacher pt in user.PendingTeachers)
                    {
                        context.Entry(pt).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    }

                    HttpContext.Session.SetObject("theUser", user);
                    context.SaveChanges();
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                }
                return user;
            }

            //if new user
            else if (user != null && user.Groups != null && user.Groups.Count == 1)
            {
                bool success = context.TeacherRegister(user);

                if (success)
                {
                    HttpContext.Session.SetObject("theUser", user);
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

                    return user;
                }

                else
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                    return null;
                }
            }

            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }


        }

        [Route("ParentRegister")]
        [HttpPost]
        public User ParentRegister([FromBody] RegisterUserDto register)
        {
            //If user is null the request is bad
            if (register == null || register.User == null || register.Student == null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return null;
            }

            User currentUser = HttpContext.Session.GetObject<User>("theUser");
            User user = register.User;
            Student student = register.Student;

            //Check if user logged in and its ID is the same as the contact user ID
            if (currentUser != null && currentUser.UserId == user.UserId)
            {
                //delete all allergies before adding them back for the student
                List<StudentAllergy> allergies = context.StudentAllergies.Where(al => al.StudentId == student.StudentId).ToList();
                foreach (StudentAllergy sa in allergies)
                {
                    context.Entry(sa).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
                context.SaveChanges();

                try
                {
                    //Update user info
                    context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                    context.Entry(student).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    foreach (StudentAllergy alerrgy in student.StudentAllergies)
                    {
                        alerrgy.AllergyId = alerrgy.Allergy.AllergyId;
                        context.Entry(alerrgy).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    }

                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    foreach (StudentAllergy sa in allergies)
                    {
                        context.Entry(sa).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    }
                    context.SaveChanges();
                }


                HttpContext.Session.SetObject("theUser", user);


                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

                return user;
            }


            else if (user != null)
            {
                //Create user password
                if (string.IsNullOrEmpty(user.Password))
                {
                    user.Password = GenerateAlphanumerical(6);
                    context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    context.Entry(user.StudentOfUsers.Last()).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    context.SaveChanges();

                    EmailSender.SendEmail2("ברוכים הבאים!", $"  {user.Password} - סיסמתך לאפליקציה  ", $"{user.Email}", $"{user.Fname} {user.LastName}", "<ganenu1@gmail.com>", $"גננו", "#GANENU123!", "smtp.gmail.com");
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return user;
                }

                bool success = context.ParentRegister(user, student);

                if (success)
                {
                    HttpContext.Session.SetObject("theUser", user);
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return user;
                }
                else
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                    return null;
                }
            }

            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }
        }

    }




}
