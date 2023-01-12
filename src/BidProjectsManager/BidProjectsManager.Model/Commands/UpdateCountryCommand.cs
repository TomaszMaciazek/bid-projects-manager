namespace BidProjectsManager.Model.Commands
{
    public class UpdateCountryCommand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int CurrencyId { get; set; }
    }
}
