using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BidProjectsManager.Model.Entities
{
    public class BidCapex : BaseEntity
    {
        [Required]
        public int Year { get; set; }
        public decimal? Value { get; set; }
        [Required]
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
