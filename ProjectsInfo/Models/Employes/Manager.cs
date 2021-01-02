using System.Collections.Generic;
using ProjectsInfo.Models.employes;

namespace ProjectsInfo.Models.Employes
{
    public class Manager : Employee
    {
        public IEnumerable<Project> Projects { get; set; }
    }
}