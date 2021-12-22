using System;
using System.Collections.Generic;

#nullable disable

namespace MyGanServerBL
{
    public partial class Kindergarten
    {
        public Kindergarten()
        {
            Groups = new HashSet<Group>();
            KindergartenManagers = new HashSet<KindergartenManager>();
        }

        public int KindergartenId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<KindergartenManager> KindergartenManagers { get; set; }
    }
}
