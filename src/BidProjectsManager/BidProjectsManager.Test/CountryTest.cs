using BidProjectsManager.DataLayer.Common;
using BidProjectsManager.Model.Commands;
using BidProjectsManager.Model.Entities;
using BidProjectsManager.Test.Helpers;
using BidProjectsManager.Validation.Validators;
using Microsoft.EntityFrameworkCore;

namespace BidProjectsManager.Test
{
    public class CountryTest
    {

        private readonly IUnitOfWork unitOfWork;

        public CountryTest()
        {
            var helper = new DbHelper();
            unitOfWork = helper.GetInMemoryUnitOfWork();

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

            unitOfWork.CurrencyRepository.AddRange(currencies);
            unitOfWork.SaveChanges();

            var countries = new List<Country>
            {
                new Country{Id = 1, Code = "POL", CurrencyId = 1, Name = "Poland"},
                new Country{Id = 2, Code = "DEU", CurrencyId = 2, Name = "Germany"},
                new Country{Id = 3, Code = "CZE", CurrencyId = 3, Name = "Czech Republic"},
                new Country{Id = 4, Code = "DNK", CurrencyId = 6, Name = "Denmark"},
                new Country{Id = 5, Code = "ITA", CurrencyId = 2, Name = "Italy"},
                new Country{Id = 6, Code = "SVK", CurrencyId = 2, Name = "Slovakia"},
                new Country{Id = 7, Code = "AUT", CurrencyId = 2, Name = "Austria"},
                new Country{Id = 8, Code = "HRV", CurrencyId = 5, Name = "Croatia"},
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

            unitOfWork.CountryRepository.AddRange(countries);
            unitOfWork.SaveChanges();
        }

        [Fact]
        public void GetCountriesTest()
        {
            var countries = unitOfWork.CountryRepository.GetAll().AsNoTracking().ToList();
            Assert.True(countries.Any());
        }

        [Theory]
        [InlineData("Test1", "CO1", 1, true)]
        [InlineData("Test2", "", 2, false)]
        [InlineData("", "CO3", 3, false)]
        [InlineData("Test4", "CO4", 41, false)]
        public async Task CreateCountryCommandCorrectValidationTest(string name, string code, int currencyId, bool expectedResult)
        {
            var validator = new CreateCountryCommandValidator(unitOfWork);

            var command = new CreateCountryCommand
            {
                Code = code,
                CurrencyId = currencyId,
                Name = name,
            };


            var validationResult = await validator.ValidateAsync(command);

            Assert.True(validationResult.IsValid == expectedResult);
        }
    }
}