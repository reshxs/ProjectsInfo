using System;

namespace ProjectsInfo.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FamalyName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        //TODO добавить отношение Mane-to-One с проектом
    }
}
