using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectsInfo.Models
{
    public class Month
    {
        public int ID { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Часы работы")]
        public int Hours { get; set; }

        [Display(Name = "Разработчик")]
        public ProjectAssigment ProjectAssigment { get; set; }
    }
}
