using System.ComponentModel.DataAnnotations;

namespace BidProjectsManager.Model.Entities
{
    public class Currency : BaseEntity
    {
        [Required]
        [MaxLength(3)]
        public string Code { get; set; }

        public ICollection<Country> Countries { get; set; }
        public ICollection<Project> Projects { get; set; }
    }
}
