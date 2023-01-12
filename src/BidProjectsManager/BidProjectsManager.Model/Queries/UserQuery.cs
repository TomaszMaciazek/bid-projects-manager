using BidProjectsManager.Model.Enums;

namespace BidProjectsManager.Model.Queries
{
    public class UserQuery
    {
        public string Name { get; set; }
        public UserRole? Role { get; set; }
    }
}
