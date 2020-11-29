using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectsInfo.Models
{
    public class Month
    {
        public int DeveloperAssignmentID { get; set; }
        public DeveloperAssignment DeveloperAssignment { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Часы работы")]
        public int Hours { get; set; }
    }
}
