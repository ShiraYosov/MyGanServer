﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyGanServerBL.Models;
using MyGanServer.DTO;
using System.IO;

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


        [Route("Login")]
        [HttpGet]
        public User Login([FromQuery] string email, [FromQuery] string pass)
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

        [Route("GetTeachersWithWaitStatus")]
        [HttpGet]
        public List<PendingTeacher> GetTeachersWithWaitStatus([FromQuery] int kindergartenID)
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


        [Route("ChangeTeacherStatus")]
        [HttpPost]
        public bool ChangeTeacherStatus(PendingTeacher t)
        {
            User user = HttpContext.Session.GetObject<User>("theUser");

            if (user != null)
            {

                bool ok = this.context.ChangeStatusForUser(t);

                if (ok)
                {
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

        [Route("ChangeParentStatus")]
        [HttpPost]
        public bool ChangeParentStatus(StudentOfUser s)
        {
            User user = HttpContext.Session.GetObject<User>("theUser");

            if (user != null)
            {

                bool ok = this.context.ChangeStatusForUser(s);

                if (ok)
                {
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

        public bool AddGroup([FromBody] Group group)
        {
            User user = HttpContext.Session.GetObject<User>("theUser");

            if (user != null)
            {
                if (!context.Groups.Contains(group))
                {
                    context.Entry(group).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    context.SaveChanges();

                }
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return true;
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
            User user = HttpContext.Session.GetObject<User>("theUser");

            if (user != null)
            {
                if (allergy != null)
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
                else
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return false;
                }
            }
            else
            {

                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return false;
            }

        }

        //[Route("AddGroup")]
        //[HttpPost]
        //public bool AddGroup([FromBody] Group group)
        //{
        //    User user = HttpContext.Session.GetObject<User>("theUser");

        //    if (user != null)
        //    {
        //        if (group != null)
        //        {
        //            bool added = this.context.AddGroup(group);
        //            if (added)
        //            {
        //                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
        //                return added;
        //            }
        //            else
        //                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
        //            return false;
        //        }
        //        else
        //        {
        //            Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
        //            return false;
        //        }
        //    }
        //    else
        //    {

        //        Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
        //        return false;
        //    }

        //}

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
