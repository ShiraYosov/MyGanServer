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
            Users = new HashSet<User>();
        }

        public int StatusId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Approval> Approvals { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
