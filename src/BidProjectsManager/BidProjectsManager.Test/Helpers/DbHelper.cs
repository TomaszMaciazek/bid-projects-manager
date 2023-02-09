using BidProjectsManager.DataLayer;
using BidProjectsManager.DataLayer.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidProjectsManager.Test.Helpers
{
    public class DbHelper
    {
        private readonly ApplicationDbContext _context;

        public DbHelper()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase(databaseName: "Bid-Projects-Db-InMemory");

            var dbContextOptions = builder.Options;
            _context = new ApplicationDbContext(dbContextOptions);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        public IUnitOfWork GetInMemoryUnitOfWork()
        {
            return new UnitOfWork(_context);
        }
    }
}
