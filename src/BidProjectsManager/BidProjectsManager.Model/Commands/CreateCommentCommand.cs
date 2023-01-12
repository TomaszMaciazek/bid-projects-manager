using System.ComponentModel.DataAnnotations;

namespace BidProjectsManager.Model.Commands
{
    public class CreateCommentCommand
    {
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public int ProjectId { get; set; }
        public string Content { get; set; }
    }
}
