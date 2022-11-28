using BidProjectsManager.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidProjectsManager.DataLayer
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<BidCapex> BidCapexes { get; set; }
        public DbSet<BidEbit> BidEbits { get; set; }
        public DbSet<BidOpex> BidOpexes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectComment> Comments { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BidCapex>()
                .HasOne(c => c.Project)
                .WithMany(p => p.Capexes)
                .HasForeignKey(c => c.ProjectId);

            builder.Entity<BidEbit>()
                .HasOne(c => c.Project)
                .WithMany(p => p.Ebits)
                .HasForeignKey(c => c.ProjectId);

            builder.Entity<BidOpex>()
                .HasOne(c => c.Project)
                .WithMany(p => p.Opexes)
                .HasForeignKey(c => c.ProjectId);

            builder.Entity<Country>()
                .HasMany(c => c.Projects)
                .WithOne(p => p.Country)
                .HasForeignKey(c => c.CountryId);

            builder.Entity<Currency>()
                .HasMany(c => c.Countries)
                .WithOne(p => p.Currency)
                .HasForeignKey(c => c.CurrencyId);

            builder.Entity<Currency>()
                .HasMany(c => c.Projects)
                .WithOne(p => p.Currency)
                .HasForeignKey(c => c.CurrencyId);

            builder.Entity<Project>()
                .HasMany(c => c.Comments)
                .WithOne(p => p.Project);

        }
    }
}
