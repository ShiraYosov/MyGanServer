using System;
using System.Collections.Generic;

#nullable disable

namespace MyGanServerBL.Models
{
    public partial class Approval
    {
        public Approval()
        {
            Signatures = new HashSet<Signature>();
        }

        public int ApprovalId { get; set; }
        public int Waiting { get; set; }
        public int Approved { get; set; }
        public int StatusId { get; set; }

        public virtual Group ApprovalNavigation { get; set; }
        public virtual StatusType Status { get; set; }
        public virtual ICollection<Signature> Signatures { get; set; }
    }
}
