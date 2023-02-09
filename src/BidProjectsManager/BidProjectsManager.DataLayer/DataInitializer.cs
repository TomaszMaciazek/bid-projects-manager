using BidProjectsManager.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BidProjectsManager.DataLayer
{
    public interface IDataInitializer
    {
        Task MigrateAsync();
        Task SeedAsync();
    }

    public class DataInitializer : IDataInitializer
    {
        private readonly ApplicationDbContext _context;

        public DataInitializer(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task MigrateAsync()
        {
            await _context.Database.MigrateAsync();
        }

        public async Task SeedAsync()
        {
            if (!await _context.Currencies.AnyAsync())
            {
                using var transaction = _context.Database.BeginTransaction();
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Currencies] ON");
                var currencies = new List<Currency>
                {
                    new Currency{ Id = 1, Code = "PLN", Name = "Polish Zloty"},
                    new Currency{ Id = 2, Code = "EUR", Name = "Euro"},
                    new Currency{ Id = 3, Code = "CZK", Name = "Czech Koruna"},
                    new Currency{ Id = 4, Code = "BGN", Name = "Bulgarian Lev"},
                    new Currency{ Id = 5, Code = "HRK", Name = "Croatian Kuna"},
                    new Currency{ Id = 6, Code = "DKK", Name = "Danish Krone"},
                    new Currency{ Id = 7, Code = "HUF", Name = "Hungarian Forint"},
                    new Currency{ Id = 8, Code = "CHF", Name = "Swiss Franc"},
                    new Currency{ Id = 9, Code = "NOK", Name = "Norwegian Krone"},
                    new Currency{ Id = 10, Code = "SEK", Name = "Swedish Krona"},
                    new Currency{ Id = 11, Code = "TRY", Name = "Turkish Lira"}
                };

                _context.Currencies.AddRange(currencies);
                await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Currencies] OFF");
                await transaction.CommitAsync();
            }

            if (!await _context.Countries.AnyAsync())
            {
                using var transaction = _context.Database.BeginTransaction();
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Countries] ON");
                var countries = new List<Country>
                    {
                        new Country{Id = 1, Code = "POL", CurrencyId = 1, Name = "Poland"},
                        new Country{Id = 2, Code = "DEU", CurrencyId = 2, Name = "Germany"},
                        new Country{Id = 3, Code = "CZE", CurrencyId = 3, Name = "Czech Republic"},
                        new Country{Id = 4, Code = "DNK", CurrencyId = 6, Name = "Denmark"},
                        new Country{Id = 5, Code = "ITA", CurrencyId = 2, Name = "Italy"},
                        new Country{Id = 6, Code = "SVK", CurrencyId = 2, Name = "Slovakia"},
                        new Country{Id = 7, Code = "AUT", CurrencyId = 2, Name = "Austria"},
                        new Country{Id = 8, Code = "HRV", CurrencyId = 5, Name = "Croatia"}, //on January 2023 Croatia changes currency to Euro
                        new Country{Id = 9, Code = "SVN", CurrencyId = 2, Name = "Slovenia"},
                        new Country{Id = 10, Code = "FRA", CurrencyId = 2, Name = "France"},
                        new Country{Id = 11, Code = "FIN", CurrencyId = 2, Name = "Finland"},
                        new Country{Id = 12, Code = "GRC", CurrencyId = 2, Name = "Greece"},
                        new Country{Id = 13, Code = "CHE", CurrencyId = 8, Name = "Switzerland"},
                        new Country{Id = 14, Code = "ESP", CurrencyId = 2, Name = "Spain"},
                        new Country{Id = 15, Code = "SWE", CurrencyId = 10, Name = "Sweden"},
                        new Country{Id = 16, Code = "NOR", CurrencyId = 9, Name = "Norway"},
                        new Country{Id = 17, Code = "TUR", CurrencyId = 11, Name = "Turkey"},
                        new Country{Id = 18, Code = "HUN", CurrencyId = 7, Name = "Hungary"},
                        new Country{Id = 19, Code = "BGR", CurrencyId = 4, Name = "Bulgaria"}
                    };

                _context.Countries.AddRange(countries);
                await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Countries] OFF");
                await transaction.CommitAsync();
            }

            if (!await _context.DictionaryTypes.AnyAsync())
            {
                using var transaction = _context.Database.BeginTransaction();
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[DictionaryTypes] ON");
                var bidPriorities = new List<DictionaryType> {
                        new DictionaryType { Id = 1, Name = "Project Stage"},
                    new DictionaryType { Id = 2, Name = "Bid Status"},
                    new DictionaryType{ Id = 3, Name = "Priority"},
                    new DictionaryType{ Id = 4, Name = "Probability"},
                    new DictionaryType{ Id = 5, Name = "Project Type"}
                    };
                _context.DictionaryTypes.AddRange(bidPriorities);
                await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[DictionaryTypes] OFF");
                await transaction.CommitAsync();
            }

            if (!await _context.DictionaryValues.AnyAsync())
            {
                var dictionaryValues = new List<DictionaryValue>
                {
                    new DictionaryValue { DictionaryTypeId = 1, Value= 1, Description =  "Draft"},
                    new DictionaryValue { DictionaryTypeId = 1, Value= 2, Description =  "Submitted"},
                    new DictionaryValue { DictionaryTypeId = 1, Value= 3, Description =  "Approved"},
                    new DictionaryValue { DictionaryTypeId = 1, Value= 4, Description =  "Rejected"},
                    new DictionaryValue { DictionaryTypeId = 2, Value= 1, Description =  "Bid Preparation"},
                    new DictionaryValue { DictionaryTypeId = 2, Value= 2, Description =  "Won"},
                    new DictionaryValue { DictionaryTypeId = 2, Value= 3, Description =  "Lost"},
                    new DictionaryValue { DictionaryTypeId = 2, Value= 4, Description =  "No Bid"},
                    new DictionaryValue { DictionaryTypeId = 2, Value= 5, Description =  "Awaiting Signature"},
                    new DictionaryValue { DictionaryTypeId = 3, Value= 1, Description =  "Low"},
                    new DictionaryValue { DictionaryTypeId = 3, Value= 2, Description =  "Medium"},
                    new DictionaryValue { DictionaryTypeId = 3, Value= 3, Description =  "High"},
                    new DictionaryValue { DictionaryTypeId = 4, Value= 1, Description =  "Low"},
                    new DictionaryValue { DictionaryTypeId = 4, Value= 2, Description =  "Medium"},
                    new DictionaryValue { DictionaryTypeId = 4, Value= 3, Description =  "High"},
                    new DictionaryValue { DictionaryTypeId = 5, Value= 1, Description =  "Tender Offer"},
                    new DictionaryValue { DictionaryTypeId = 5, Value= 2, Description =  "Acquisition"}
                };
                _context.DictionaryValues.AddRange(dictionaryValues);
                await _context.SaveChangesAsync();
            }
        }
    }
}
