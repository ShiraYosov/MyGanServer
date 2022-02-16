using System;
using System.Collections.Generic;

#nullable disable

namespace MyGanServerBL.Models
{
    public partial class StudentOfUser
    {
        public string StudentId { get; set; }
        public int UserId { get; set; }
        public int RelationToStudentId { get; set; }
        public bool Vaad { get; set; }
        public int? StatusId { get; set; }

        public virtual RelationToStudent RelationToStudent { get; set; }
        public virtual StatusType Status { get; set; }
        public virtual Student Student { get; set; }
        public virtual User User { get; set; }
    }
}
