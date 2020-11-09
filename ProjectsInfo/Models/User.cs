using System;
namespace ProjectsInfo.Models
{
    public class User
    {
        public User()
        {
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FamalyName { get; set; }
        public decimal Salary { get; set; }
    }
}
