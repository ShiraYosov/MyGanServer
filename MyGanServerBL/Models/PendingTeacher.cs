using System;
using System.Collections.Generic;

#nullable disable

namespace MyGanServerBL.Models
{
    public partial class PendingTeacher
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public int StatusId { get; set; }

        public virtual Group Group { get; set; }
        public virtual StatusType Status { get; set; }
        public virtual User User { get; set; }
    }
}
