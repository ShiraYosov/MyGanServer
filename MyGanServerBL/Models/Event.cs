using System;
using System.Collections.Generic;

#nullable disable

namespace MyGanServerBL.Models
{
    public partial class Event
    {
        public Event()
        {
            Photos = new HashSet<Photo>();
        }

        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventDate { get; set; }
        public int GroupId { get; set; }

        public virtual Group Group { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}
