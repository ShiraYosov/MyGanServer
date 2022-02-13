using Microsoft.AspNetCore.Http;
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
        public List<User> GetTeachersWithWaitStatus([FromQuery] int kindergartenID)
        {
            User user = HttpContext.Session.GetObject<User>("theUser");

            if (user != null)
            {
                List<User> teachersList = this.context.GetTeachersList(kindergartenID);
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

                return teachersList;
            }

            else
            {

                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }


        }


        [Route("ChangeUserStatus")]
        [HttpPost]
        public bool ChangeUserStatus( User u/*[FromQuery] int userID*//*,[FromQuery] int statusID*/)
        {
            User user = HttpContext.Session.GetObject<User>("theUser");

            if (user != null)
            {
                bool ok = this.context.ChangeStatusForUser(2, u);
                if(ok)
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

        [Route("AddAllergy")]
        [HttpPost]
        public bool AddAllergy([FromBody] Allergy allergy)
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

        [Route("Register")]
        [HttpPost]
        public User Register([FromBody] User user)
        {
            //Check user name and password
            if (user != null)
            {
                context.AddUser(user);

                HttpContext.Session.SetObject("theUser", user);
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                //Important! Due to the Lazy Loading, the user will be returned with all of its contects!!
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

            if (user != null && user.Groups != null && user.Groups.Count == 1)
            {
                context.TeacherRegister(user);

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

        [Route("ParentRegister")]
        [HttpPost]
        public User ParentRegister([FromBody] User user)
        {

            if (user != null)
            {
                bool success = context.ParentRegister(user);

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
