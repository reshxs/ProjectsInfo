using System.ComponentModel.DataAnnotations;

namespace ProjectsInfo.Models.employes
{
    public class Employee
    {
        // Обшие затраты - константа, прибавляющаяся к стоимости часа
        private const double GenralExpences = 100;

        // Повышающий коэффициент
        private const double Multiplier = 2.5;

        // Primary key
        public int ID { get; set; }

        [Display(Name = "ФИО")]
        public string Name { get; set; }

        [Display(Name = "Зарплата")]
        [DataType(DataType.Currency)]
        public double Salary { get; set; }

        [Display(Name = "Стоимость часа работы")]
        public double HourPrice => (Salary / 160) * Multiplier + GenralExpences;
    }
}