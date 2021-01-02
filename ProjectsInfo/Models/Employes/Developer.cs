using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProjectsInfo.Models.employes;

namespace ProjectsInfo.Models.Employes
{
    public class Developer : Employee
    {
        [Display(Name = "Проекты")]
        public ICollection<DeveloperAssignment> DeveloperAssignments { get; set; }
    }
}
