using System.ComponentModel.DataAnnotations;

namespace BidProjectsManager.Model.Entities
{
    public class ProjectComment : BaseEntity
    {

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public string Content { get; set; }
        [Required]
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
