﻿using System;

namespace ProjectsInfo.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public bool IsManager { get; set; }
    }
}
