using System;
using System.Collections.Generic;

#nullable disable

namespace MyGanServerBL.Models
{
    public partial class Allergy
    {
        public Allergy()
        {
            StudentAllergies = new HashSet<StudentAllergy>();
        }

        public int AllergyId { get; set; }
        public string AllergyName { get; set; }

        public virtual ICollection<StudentAllergy> StudentAllergies { get; set; }
    }
}
