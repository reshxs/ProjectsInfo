﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [DataType(DataType.Currency)]
        [Display(Name = "Стоимость")]
        public decimal Price =>
            TestingHours * TestingHourPrice
            + ExpectedHours * DevelopmentHourPrice;

        [Display(Name = "Разработчики")]
        public ICollection<DeveloperAssignment> DeveloperAssignments { get; set; }
    }
}
