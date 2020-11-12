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

        [DataType(DataType.Time)]
        [Display(Name = "Часы разработки")]
        public TimeSpan ExpectedHours { get; set; }

        [Display(Name = "Цена часа разработки")]
        [DataType(DataType.Currency)]
        public decimal DevelopmentHourPrice { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Часы тестирования")]
        public TimeSpan TestingHours { get; set; }

        [Display(Name = "Цена часа тестирования")]
        [DataType(DataType.Currency)]
        public decimal TestingHourPrice { get; set; }

        [Display(Name = "Общие расходы")]
        [DataType(DataType.Currency)]
        public decimal GeneralExpences { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Стоимость")]
        public decimal Price { get {
                return TestingHours.Hours * TestingHourPrice
                    + ExpectedHours.Hours * DevelopmentHourPrice
                    + GeneralExpences;
            } }

        //TODO add list of developers with their salary and hours per each month
    }
}
