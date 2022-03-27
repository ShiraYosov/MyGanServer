using System;
using System.Collections.Generic;

#nullable disable

namespace MyGanServerBL.Models
{
    public partial class Group
    {
        public Group()
        {
            Events = new HashSet<Event>();
            Messages = new HashSet<Message>();
            PendingTeachers = new HashSet<PendingTeacher>();
            Students = new HashSet<Student>();
        }

        public int GroupId { get; set; }
        public int TeacherId { get; set; }
        public string GroupName { get; set; }
        public int KindergartenId { get; set; }

        public virtual Kindergarten Kindergarten { get; set; }
        public virtual User Teacher { get; set; }
        public virtual Approval Approval { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<PendingTeacher> PendingTeachers { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
