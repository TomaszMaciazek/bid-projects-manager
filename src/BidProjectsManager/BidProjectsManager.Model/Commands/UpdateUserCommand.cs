using BidProjectsManager.Model.Enums;

namespace BidProjectsManager.Model.Commands
{
    public class UpdateUserCommand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public IEnumerable<int> CountryIds { get; set; }
    }
}
