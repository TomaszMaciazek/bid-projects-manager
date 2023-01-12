namespace BidProjectsManager.Model.Dto
{
    public class CommentDto
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public string Content { get; set; }
    }
}
