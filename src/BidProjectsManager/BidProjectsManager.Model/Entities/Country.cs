using System.ComponentModel.DataAnnotations;

namespace BidProjectsManager.Model.Entities
{
    public class Country : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [MaxLength(3)]
        public string Code { get; set; }

        [Required]
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<User> Users { get; set; }

    }
}
