using System;
namespace ProjectsInfo.Models
{
    public class ProjectAssigment
    {
        public int ID { get; set; }
        public Project Project { get; set; }
        public Developer Developer { get; set; }
    }
}
