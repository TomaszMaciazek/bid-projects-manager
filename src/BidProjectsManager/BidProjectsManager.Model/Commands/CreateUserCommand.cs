using BidProjectsManager.Model.Enums;

namespace BidProjectsManager.Model.Commands
{
    public class CreateUserCommand
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public IEnumerable<int> CountryIds { get; set; }
    }
}
