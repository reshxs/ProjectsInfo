using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectsInfo.Models
{
    public class Project
    {
        public int ID { get; set; }
        [Display(Name = "Название")]
        public string Title { get; set; }

        [Display(Name = "Дата начала")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Дата окончания")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Часы разработки")]
        public TimeSpan ExpectedHours { get; set; }
        [Display(Name = "Цена часа разработки")]
        public decimal DevelopmentHourPrice { get; set; }
        [Display(Name = "Часы тестирования")]
        public TimeSpan TestingHours { get; set; }
        [Display(Name = "Цена часа тестирования")]
        public decimal TestingHourPrice { get; set; }

        [Display(Name = "Общие расходы")]
        public decimal GeneralExpences { get; set; }

        //TODO add list of developers with their salary and hours per each month
    }
}
