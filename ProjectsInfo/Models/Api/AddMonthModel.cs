namespace ProjectsInfo.Models.Api
{
    public class AddMonthModel
    {
        public int DeveloperId { get; set; }
        public int ProjectId { get; set; }
        public string Date { get; set; }
        public int Hours { get; set; }
    }
}