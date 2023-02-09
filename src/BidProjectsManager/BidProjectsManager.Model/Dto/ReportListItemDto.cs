using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidProjectsManager.Model.Dto
{
    public class ReportListItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Group { get; set; }
        public bool IsActive { get; set; }
        public string Version { get; set; }
    }
}
