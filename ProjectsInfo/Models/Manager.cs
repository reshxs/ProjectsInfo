using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectsInfo.Models
{
    public class Manager
    {
        public int ID { get; set; }
        [Display(Name = "ФИО")]
        public string Name { get; set; }
        
        [DataType(DataType.Currency)]
        [Display(Name = "Зарплата")]
        public decimal Salary { get; set; }
        
        public IEnumerable<Project> Projects { get; set; }
    }
}