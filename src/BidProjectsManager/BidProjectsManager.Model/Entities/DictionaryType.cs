namespace BidProjectsManager.Model.Entities
{
    public class DictionaryType : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<DictionaryValue> Values { get; set; }
    }
}
