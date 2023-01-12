using System.ComponentModel.DataAnnotations;

namespace BidProjectsManager.Model.Entities
{
    public class BidOpex : BaseEntity
    {
        [Required]
        public int Year { get; set; }
        public decimal? Value { get; set; }
        [Required]
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
