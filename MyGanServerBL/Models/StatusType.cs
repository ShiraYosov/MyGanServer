using System;
using System.Collections.Generic;

#nullable disable

namespace MyGanServerBL.Models
{
    public partial class StatusType
    {
        public StatusType()
        {
            Approvals = new HashSet<Approval>();
            PendingTeachers = new HashSet<PendingTeacher>();
            StudentOfUsers = new HashSet<StudentOfUser>();
        }

        public int StatusId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Approval> Approvals { get; set; }
        public virtual ICollection<PendingTeacher> PendingTeachers { get; set; }
        public virtual ICollection<StudentOfUser> StudentOfUsers { get; set; }
    }
}
