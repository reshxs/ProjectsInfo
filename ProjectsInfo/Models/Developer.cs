using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectsInfo.Models
{
    public class Developer
    {
        // Обшие затраты - константа, прибавляющаяся к стоимости часа
        private const decimal GenralExpences = 100;

        // Повышающий коэффициент
        private const decimal Multiplier = 2.5m;

        public int ID { get; set; }

        [Display(Name = "ФИО")]
        public string Name { get; set; }

        [Display(Name = "Зарплата")]
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [Display(Name = "Стоимость часа работы")]
        public decimal HourPrice { get
            {
                return (Salary / 160) * Multiplier + GenralExpences;
            }}

        [Display(Name = "Проекты")]
        public ICollection<ProjectAssigment> Projects { get; set; }
    }
}
