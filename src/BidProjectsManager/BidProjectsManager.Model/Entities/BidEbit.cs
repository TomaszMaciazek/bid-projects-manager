using System.ComponentModel.DataAnnotations;

namespace BidProjectsManager.Model.Entities
{
    public class BidEbit : BaseEntity
    {
        [Required]
        public int Year { get; set; }
        [Required]
        public decimal Value { get; set; }
        [Required]
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
