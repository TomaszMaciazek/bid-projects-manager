using System.ComponentModel.DataAnnotations;

namespace BidProjectsManager.Model.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
