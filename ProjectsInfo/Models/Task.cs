using System;
namespace ProjectsInfo.Models
{
    public class Task
    {
        public Task()
        {
        }

        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
