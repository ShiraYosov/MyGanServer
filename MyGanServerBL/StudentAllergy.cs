using System;
using System.Collections.Generic;

#nullable disable

namespace MyGanServerBL
{
    public partial class StudentAllergy
    {
        public int StudentId { get; set; }
        public int AllergyId { get; set; }

        public virtual Allergy Allergy { get; set; }
        public virtual Student Student { get; set; }
    }
}
