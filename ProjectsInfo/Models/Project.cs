using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ProjectsInfo.Models.Employes;

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

        public int MonthsCount => (DateTime.Now.Month - StartDate.Month + 1) + 12 * (DateTime.Now.Year - StartDate.Year);

        public IEnumerable<string> MonthCollection
        {
            get
            {
                var result = new List<string>();
                for (var i = 0; i < MonthsCount; i++)
                {
                    var month = StartDate.Month + i % 12;
                    var year = StartDate.Year + i / 12;
                    result.Add($"{month}/{year}");
                }

                return result;
            }
        }

        public IEnumerable<int> MonthsTotals
        {
            get
            {
                if (DeveloperAssignments == null)
                    return null;
                var result = new List<int>(MonthsCount);
                foreach (var developer in DeveloperAssignments)
                {
                    for (var i = 0; i < developer.Months.Count; i++)
                    {
                        var month = developer.Months.ElementAtOrDefault(i);
                        if (month != null)
                        {
                            if (result.Count < i + 1)
                            {
                                result.Add(month.Hours);
                            }
                            else
                            {
                                result[i] += month.Hours;
                            }
                        }
                    }
                }
                return result;
            }
        }

        public int TotalHours => DeveloperAssignments?.Sum(d => d.TotalHours) ?? 0;

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
        public decimal  ActualPrice
        {
            get
            {
                if (DeveloperAssignments != null)
                    return (decimal) DeveloperAssignments.Sum(developerAssignment
                        => developerAssignment.TotalHours * developerAssignment.Developer.HourPrice);
                return 0;
            }
        }

        [Display(Name = "Рентабельность (Врем.)")]
        public decimal TimeProfit
        {
            get
            {
                if(DeveloperAssignments == null)
                    return 0;
                var actualTime = DeveloperAssignments.Sum(d => d.TotalHours);
                if (actualTime == 0)
                    return 0;
                var expectedTime = ExpectedHours + TestingHours;
                var profit = (decimal)expectedTime / actualTime * 100;
                return profit;
            }
        }

        [Display(Name= "Рентабельность (Денеж.)")]
        public decimal MoneyProfit
        {
            get
            {
                if (ActualPrice == 0)
                    return 0;
                return Price / ActualPrice * 100;
            }
        }

        private static string DaysFormat(int daysCount)
        {
            var isExpired = daysCount < 0 ? "Просрочено на " : "";
            string daysFormat;
            daysCount = Math.Abs(daysCount);
            if (daysCount >= 11 && daysCount <= 19)
            {
                daysFormat = "дней";
            }
            else if (daysCount % 10 == 1)
            {
                daysFormat = "день";
            }
            else if (daysCount % 10 >= 2 && daysCount % 10 <= 4)
            {
                daysFormat = "дня";
            }
            else
            {
                daysFormat = "дней";
            }
            
            return $"{isExpired}{daysCount} {daysFormat}";
        }
    }
}
