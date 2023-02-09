namespace BidProjectsManager.Model.Dto
{
    public class GeneratedReportDto
    {
        public DateTime GenerationDate { get; set; }
        public List<string> Columns { get; set; }
        public List<string[]> Rows { get; set; }
    }
}
