using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Authentication;

namespace ProjectsInfo.Models
{
    public class DeveloperAssignment
    {
        public int ID => ProjectID + DeveloperID;

        public int ProjectID { get; set; }
        public int DeveloperID { get; set; }
        public Project Project { get; set; }
        public Developer Developer { get; set; }

        [Display(Name = "Месяцы работы")]
        public ICollection<Month> Months { get; set; }
    }
}
