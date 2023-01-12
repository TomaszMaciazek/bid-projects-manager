namespace BidProjectsManager.Model.Entities
{
    public class DictionaryValue : BaseEntity
    {
        public int DictionaryTypeId { get; set; }
        public DictionaryType Type { get; set; }
        public int Value { get; set; }
        public string Description { get; set; }
    }
}
