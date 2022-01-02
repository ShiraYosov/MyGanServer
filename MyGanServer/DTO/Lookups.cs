using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyGanServerBL.Models;
namespace MyGanServer.DTO
{
    public class Lookups
    {
        public List<Grade> Grades { get; set; }
        public List<Allergy> Allergies { get; set; }
        public List<RelationToStudent> Relations { get; set; }
    }
}
