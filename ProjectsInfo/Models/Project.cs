using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ProjectsInfo.Models
{
    public class Project
    {        
        // Primary key
        public int ID { get; set; }

        [Display(Name = "Название")]
        public string Title { get; set; }

        [Display(Name = "Дата начала")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Дата окончания")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "До конца")] 
        public string TimeBeforeEnd => DaysFormat((EndDate - DateTime.Now).Days);

        [Display(Name = "Часы разработки")]
        public int ExpectedHours { get; set; }

        [Display(Name = "Цена часа разработки")]
        [DataType(DataType.Currency)]
        public decimal DevelopmentHourPrice { get; set; }

        [Display(Name = "Часы тестирования")]
        public int TestingHours { get; set; }

        [Display(Name = "Цена часа тестирования")]
        [DataType(DataType.Currency)]
        public decimal TestingHourPrice { get; set; }

        [Display(Name = "Разработчики")]
        public ICollection<DeveloperAssignment> DeveloperAssignments { get; set; }
        
        public int? ManagerID { get; set; }
        [Display(Name = "Менеджер")]
        public Manager Manager { get; set; }
        
        [DataType(DataType.Currency)]
        [Display(Name = "Стоимость")]
        public decimal Price =>
            TestingHours * TestingHourPrice
            + ExpectedHours * DevelopmentHourPrice;

        [DataType(DataType.Currency)]
        [Display(Name = "Фактическая цена")]
        public decimal ActualPrice
        {
            get
            {
                if (DeveloperAssignments != null)
                    return DeveloperAssignments.Sum(developerAssignment
                        => developerAssignment.TotalHours * developerAssignment.Developer.HourPrice);
                return 0;
            }
        }

        private static string DaysFormat(int daysCount)
        {
            string daysFormat;
            if (daysCount >= 11 && daysCount <= 19)
            {
                daysFormat = "дней";
            }
            if (daysCount % 10 == 1)
            {
                daysFormat = "день";
            }
            if (daysCount % 10 >= 2 && daysCount % 10 <= 4)
            {
                daysFormat = "дня";
            }
            else
            {
                daysFormat = "дней";
            }

            return $"{daysCount} {daysFormat}";
        }
    }
}
