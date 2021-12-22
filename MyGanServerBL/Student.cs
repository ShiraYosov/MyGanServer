﻿using System;
using System.Collections.Generic;

#nullable disable

namespace MyGanServerBL
{
    public partial class Student
    {
        public Student()
        {
            StudentAllergies = new HashSet<StudentAllergy>();
            StudentOfUsers = new HashSet<StudentOfUser>();
        }

        public int StudentId { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public int GradeId { get; set; }
        public int GroupId { get; set; }

        public virtual Grade Grade { get; set; }
        public virtual Group Group { get; set; }
        public virtual ICollection<StudentAllergy> StudentAllergies { get; set; }
        public virtual ICollection<StudentOfUser> StudentOfUsers { get; set; }
    }
}

