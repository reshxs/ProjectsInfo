using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectsInfo.Models
{
    public class ProjectTask
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        //TODO добавить отношение Many-to-One с проектом
        //TODO добавить отношение One-to-One с разработчиком
    }
}
