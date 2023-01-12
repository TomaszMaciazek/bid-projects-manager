using System.ComponentModel.DataAnnotations;

namespace BidProjectsManager.Model.Commands
{
    public class CreateCountryCommand
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int CurrencyId { get; set; }
    }
}
