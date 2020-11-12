using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectsInfo.Models
{
    public class Project
    {
        public int ID { get; set; }
        public string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public TimeSpan ExpectedHours { get; set; }
        public TimeSpan TestingHours { get; set; }

        public decimal GeneralExpences { get; set; }

        //TODO add list of developers with their salary and hours per each month
    }
}
