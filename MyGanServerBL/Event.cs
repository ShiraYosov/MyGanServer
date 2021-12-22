using System;
using System.Collections.Generic;

#nullable disable

namespace MyGanServerBL
{
    public partial class Event
    {
        public Event()
        {
            Photos = new HashSet<Photo>();
        }

        public int EventId { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public int Duration { get; set; }
        public int GroupId { get; set; }

        public virtual Group Group { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}
