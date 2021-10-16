using System;
using System.Collections.Generic;

#nullable disable

namespace MyGanServerBL.Models
{
    public partial class Signature
    {
        public int ApprovalId { get; set; }
        public int UserId { get; set; }
        public DateTime SignatureDate { get; set; }

        public virtual Approval Approval { get; set; }
        public virtual User User { get; set; }
    }
}
