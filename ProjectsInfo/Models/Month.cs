using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectsInfo.Models
{
    public class Month
    {
        //TODO: refactor this
        // Primary key
        public int ID { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Часы работы")]
        public int Hours { get; set; }

        //One-to-Many relation with ProjectAssigment
        [Display(Name = "Разработчик")]
        public DeveloperAssignment DeveloperAssignment { get; set; }
    }
}
