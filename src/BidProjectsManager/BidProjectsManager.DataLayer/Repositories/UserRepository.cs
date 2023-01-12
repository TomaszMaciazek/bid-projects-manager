using BidProjectsManager.DataLayer.Repositories.Common;
using BidProjectsManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidProjectsManager.DataLayer.Repositories
{
    public interface IUserRepository : IRepository<User> { }

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
