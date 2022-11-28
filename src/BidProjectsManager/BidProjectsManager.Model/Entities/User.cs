using BidProjectsManager.Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace BidProjectsManager.Model.Entities
{
    public class User : BaseEntity
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public ICollection<Country> Countries { get; set; }
    }
}
