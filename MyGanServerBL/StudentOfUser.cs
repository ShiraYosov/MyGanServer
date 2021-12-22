using System;
using System.Collections.Generic;

#nullable disable

namespace MyGanServerBL
{
    public partial class StudentOfUser
    {
        public int StudentId { get; set; }
        public int UserId { get; set; }
        public int RelationToStudentId { get; set; }
        public bool Vaad { get; set; }

        public virtual RelationToStudent RelationToStudent { get; set; }
        public virtual Student Student { get; set; }
        public virtual User User { get; set; }
    }
}
