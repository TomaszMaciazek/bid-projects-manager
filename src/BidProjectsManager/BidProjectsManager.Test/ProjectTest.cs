using BidProjectsManager.DataLayer.Common;
using BidProjectsManager.Model.Commands;
using BidProjectsManager.Model.Entities;
using BidProjectsManager.Test.Helpers;
using BidProjectsManager.Validation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidProjectsManager.Test
{
    public class ProjectTest
    {
        private readonly IUnitOfWork unitOfWork;

        public ProjectTest()
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

        public static IEnumerable<object[]> GetCreateDraftsCommands()
        {
            yield return new object[] { new CreateDraftProjectCommand { CountryId = 1, CurrencyId = 1, Name = "TEST" }, true };

            yield return new object[] { new CreateDraftProjectCommand { CountryId = -1, CurrencyId = 1, Name = "TEST" }, false };

            yield return new object[] { new CreateDraftProjectCommand { CountryId = 1, CurrencyId = -1, Name = "TEST" }, false };

            yield return new object[] { new CreateDraftProjectCommand { CountryId = 1, CurrencyId = 1, Name = "" }, false };
        }


        public static IEnumerable<object[]> GetCreateSubmittedProjectCommands()
        {
            yield return new object[] { new CreateSubmittedProjectCommand { 
                CountryId = 1,
                CurrencyId = 1,
                Name = "TEST",
                Description = "TEST",
                BidEstimatedOperationEnd = DateTime.Now.AddDays(2),
                BidOperationStart = DateTime.Now,
                Status = Model.Enums.BidStatus.BidPreparation,
                Probability= Model.Enums.BidProbability.High,
                NumberOfVechicles= 2,
                LifetimeInThousandsKilometers= 2,
                NoBidReason = null,
                OptionalExtensionYears= 2,
                Priority = Model.Enums.BidPriority.Medium,
                TotalCapex = 4.23m,
                TotalEbit = 4.32m,
                TotalOpex = 5.3m,
                Type = Model.Enums.ProjectType.TenderOffer,
                Opexes = new List<CreateOpexCommand>
                {
                    new CreateOpexCommand{Year = 2023, Value = 0}
                },
                Capexes = new List<CreateCapexCommand>
                {
                    new CreateCapexCommand{Year = 2023, Value = 0}
                },
                Ebits = new List<CreateEbitCommand>
                {
                    new CreateEbitCommand{Year = 2023, Value = 0}
                }
            }, true };

            yield return new object[] { new CreateSubmittedProjectCommand {
                CountryId = 1,
                CurrencyId = 1,
                Name = "",
                Description = "TEST",
                BidEstimatedOperationEnd = DateTime.Now.AddDays(2),
                BidOperationStart = DateTime.Now,
                Status = Model.Enums.BidStatus.BidPreparation,
                Probability= Model.Enums.BidProbability.High,
                NumberOfVechicles= 2,
                LifetimeInThousandsKilometers= 2,
                NoBidReason = null,
                OptionalExtensionYears= 2,
                Priority = Model.Enums.BidPriority.Medium,
                TotalCapex = 4.23m,
                TotalEbit = 4.32m,
                TotalOpex = 5.3m,
                Type = Model.Enums.ProjectType.TenderOffer,
                Opexes = new List<CreateOpexCommand>
                {
                    new CreateOpexCommand{Year = 2023, Value = 0}
                },
                Capexes = new List<CreateCapexCommand>
                {
                    new CreateCapexCommand{Year = 2023, Value = 0}
                },
                Ebits = new List<CreateEbitCommand>
                {
                    new CreateEbitCommand{Year = 2023, Value = 0}
                }
            }, false };

            yield return new object[] { new CreateSubmittedProjectCommand {
                CountryId = 1,
                CurrencyId = -1,
                Name = "TEST",
                Description = "",
                BidEstimatedOperationEnd = DateTime.Now.AddDays(2),
                BidOperationStart = DateTime.Now,
                Status = Model.Enums.BidStatus.BidPreparation,
                Probability= Model.Enums.BidProbability.High,
                NumberOfVechicles= 2,
                LifetimeInThousandsKilometers= 2,
                NoBidReason = null,
                OptionalExtensionYears= 2,
                Priority = Model.Enums.BidPriority.Medium,
                TotalCapex = 4.23m,
                TotalEbit = 4.32m,
                TotalOpex = 5.3m,
                Type = Model.Enums.ProjectType.TenderOffer,
                Opexes = new List<CreateOpexCommand>
                {
                    new CreateOpexCommand{Year = 2023, Value = 0}
                },
                Capexes = new List<CreateCapexCommand>
                {
                    new CreateCapexCommand{Year = 2023, Value = 0}
                },
                Ebits = new List<CreateEbitCommand>
                {
                    new CreateEbitCommand{Year = 2023, Value = 0}
                }
            }, false };

            yield return new object[] { new CreateSubmittedProjectCommand {
                CountryId = 1,
                CurrencyId = 11,
                Name = "TEST",
                Description = "desc",
                BidEstimatedOperationEnd = DateTime.Now.AddDays(2),
                BidOperationStart = DateTime.Now,
                Status = Model.Enums.BidStatus.BidPreparation,
                Probability= Model.Enums.BidProbability.High,
                NumberOfVechicles= 2,
                LifetimeInThousandsKilometers= 2,
                NoBidReason = null,
                OptionalExtensionYears= 2,
                Priority = Model.Enums.BidPriority.Medium,
                TotalCapex = 4.23m,
                TotalEbit = 4.32m,
                TotalOpex = 5.3m,
                Type = Model.Enums.ProjectType.TenderOffer,
                Opexes = new List<CreateOpexCommand>
                {
                    new CreateOpexCommand{Year = 0, Value = 0}
                },
                Capexes = new List<CreateCapexCommand>
                {
                    new CreateCapexCommand{Year = 0, Value = 0}
                },
                Ebits = new List<CreateEbitCommand>
                {
                    new CreateEbitCommand{Year = 0, Value = 0}
                }
            }, false };
        }

        [Theory]
        [MemberData(nameof(GetCreateDraftsCommands))]
        public async Task CreateDraftProjectCommandTest(CreateDraftProjectCommand command, bool expectedResult) {
            var validator = new CreateDraftProjectCommandValidator(unitOfWork);
            var result = await validator.ValidateAsync(command);

            Assert.True(result.IsValid == expectedResult);

        }

        [Theory]
        [MemberData(nameof(GetCreateSubmittedProjectCommands))]
        public async Task CreateSubmittedProjectCommandTest(CreateSubmittedProjectCommand command, bool expectedResult)
        {
            var validator = new CreateSubmittedProjectCommandValidator(unitOfWork);
            var result = await validator.ValidateAsync(command);
            Assert.True(result.IsValid == expectedResult);

        }
    }
}
