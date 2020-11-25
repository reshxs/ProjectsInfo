using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectsInfo.Models
{
    public class ProjectAssigment
    {
        public int ID { get; set; }

        [Display(Name = "Проект")]
        public Project Project { get; set; }

        [Display(Name = "Разработчик")]
        public Developer Developer { get; set; }

        [Display(Name = "Месяцы работы")]
        public ICollection<Month> Months { get; set; }

        [Display(Name = "Всего часов")]
        public int TotalHours
        {
            get
            {
                var total = 0;
                foreach (var month in Months)
                {
                    total += month.Hours;
                }

                return total;
            }
        }
    }
}
