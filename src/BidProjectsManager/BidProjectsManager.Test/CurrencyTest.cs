using BidProjectsManager.DataLayer.Common;
using BidProjectsManager.Model.Commands;
using BidProjectsManager.Model.Entities;
using BidProjectsManager.Test.Helpers;
using BidProjectsManager.Validation.Validators;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidProjectsManager.Test
{
    public class CurrencyTest
    {
        private readonly IUnitOfWork unitOfWork;
        public CurrencyTest() {
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
        }

        [Fact]
        public void GetCurrenciesTest()
        {
            var currencies = unitOfWork.CurrencyRepository.GetAll().AsNoTracking().ToList();
            Assert.True(currencies.Any());
        }

        [Theory]
        [InlineData("CRT","TEST1", true)]
        [InlineData("CRT2", "TEST2", false)]
        [InlineData("CRT", "", false)]
        [InlineData("", "TEST4", false)]
        public async Task CreateCurrencyTest(string code, string name, bool expectedResult)
        {
            var validator = new CreateCurrencyCommandValidator(unitOfWork);

            var command = new CreateCurrencyCommand
            {
                Code = code,
                Name = name,
            };

            var result = await validator.ValidateAsync(command);
            Assert.True(result.IsValid == expectedResult);
        }
    }
}
