using System;
using System.Collections.Generic;

#nullable disable

namespace MyGanServerBL
{
    public partial class StatusType
    {
        public StatusType()
        {
            Approvals = new HashSet<Approval>();
        }

        public int StatusId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Approval> Approvals { get; set; }
    }
}
