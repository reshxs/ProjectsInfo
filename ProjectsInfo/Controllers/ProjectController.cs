using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectsInfo.Controllers
{
    public class ProjectController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            throw new NotImplementedException();
        }

        public IActionResult ChangeFinance()
        {
            throw new NotImplementedException();
        }

        public IActionResult ChangeUsers()
        {
            throw new NotImplementedException();
        }
    }
}
