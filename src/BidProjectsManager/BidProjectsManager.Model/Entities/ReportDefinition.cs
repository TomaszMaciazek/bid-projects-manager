using System.ComponentModel.DataAnnotations;

namespace BidProjectsManager.Model.Entities
{
    public class ReportDefinition : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        [Required]

        public string Group { get; set; }

        public string Description { get; set; }

        [Required]
        public int MaxRow { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public string Version { get; set; }

        [Required]
        public string XmlDefinition { get; set; }
    }
}
