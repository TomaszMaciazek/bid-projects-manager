namespace BidProjectsManager.Model.Commands
{
    public class CreateCommentCommand
    {
        public int ProjectId { get; set; }
        public string Content { get; set; }
    }
}
