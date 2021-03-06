using System;
using System.Collections.Generic;

#nullable disable

namespace MyGanServerBL.Models
{
    public partial class Photo
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int EventId { get; set; }

        public virtual Event Event { get; set; }
        public virtual User User { get; set; }
    }
}
