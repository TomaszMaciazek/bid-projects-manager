using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidProjectsManager.Model.Commands
{
    public class UpdateReportDefinitionCommand
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public int MaxRow { get; set; }
        public string Version { get; set; }
        public bool? IsActive { get; set; }
        public string XmlDefinition { get; set; }
    }
}
