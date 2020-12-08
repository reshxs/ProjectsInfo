using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectsInfo.Models
{
    public class Manager
    {
        public int ID { get; set; }
        
        [Display(Name="ФИО")]
        public string Name { get; set; }
        
        [Display(Name="Зарплата")]
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        //TODO: добавить точную формулу
        [Display(Name = "Цена часа работы")]
        [DataType(DataType.Currency)]
        public decimal HourPrice => Salary / 160;
        
        public ICollection<Project> Projects { get; set; }
    }
}