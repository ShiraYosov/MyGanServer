using System;
using System.Collections.Generic;

#nullable disable

namespace MyGanServerBL.Models
{
    public partial class User
    {
        public User()
        {
            Groups = new HashSet<Group>();
            KindergartenManagers = new HashSet<KindergartenManager>();
            Signatures = new HashSet<Signature>();
            StudentOfUsers = new HashSet<StudentOfUser>();
        }

        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Fname { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsSystemManager { get; set; }
        public bool IsApproved { get; set; }

        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<KindergartenManager> KindergartenManagers { get; set; }
        public virtual ICollection<Signature> Signatures { get; set; }
        public virtual ICollection<StudentOfUser> StudentOfUsers { get; set; }
    }
}
