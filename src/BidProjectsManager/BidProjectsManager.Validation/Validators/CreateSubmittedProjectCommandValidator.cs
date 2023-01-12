using BidProjectsManager.DataLayer.Repositories;
using BidProjectsManager.Model.Commands;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BidProjectsManager.Validation.Validators
{
    public class CreateSubmittedProjectCommandValidator : AbstractValidator<CreateSubmittedProjectCommand>
    {
        public CreateSubmittedProjectCommandValidator(IProjectRepository projectRepository, ICountryRepository countryRepository, ICurrencyRepository currencyRepository)
        {

            RuleFor(c => c.Name)
                .NotEmpty()
                .MustAsync(async (name, cancelationToken) => {
                        return !await projectRepository.GetAll().AnyAsync(x => x.Name == name, cancellationToken: cancelationToken);
                });

            RuleFor(c => c.CurrencyId)
                .NotEmpty()
                .MustAsync(async (currencyId, cancelationToken) =>
                {
                    return !await currencyRepository.GetAll().AnyAsync(x => x.Id == currencyId, cancellationToken: cancelationToken);
                });

            RuleFor(c => c.CountryId)
                .NotEmpty()
                .MustAsync(async (countryId, cancelationToken) =>
                {
                    return !await countryRepository.GetAll().AnyAsync(x => x.Id == countryId, cancellationToken: cancelationToken);
                });

            RuleFor(c => c.NumberOfVechicles).NotEmpty();
            RuleFor(c => c.TotalEbit).NotEmpty();
            RuleFor(c => c.TotalCapex).NotEmpty();
            RuleFor(c => c.TotalOpex).NotEmpty();
            RuleFor(c => c.OptionalExtensionYears).NotEmpty();
            RuleFor(c => c.BidProbability).NotEmpty();
            RuleFor(c => c.BidEstimatedOperationEnd).NotEmpty();
            RuleFor(c => c.BidOperationStart).NotEmpty();
            RuleFor(c => c.Status).NotEmpty();
            RuleFor(c => c.CountryId).NotEmpty();
            RuleFor((c => c.Description)).NotEmpty();
            RuleFor(c => c.LifetimeInThousandsKilometers).NotEmpty();
            RuleFor(c => c.Priority).NotEmpty();

            RuleForEach(x => x.Capexes).SetValidator(new CreateCapexCommandValidator());
            RuleForEach(x => x.Ebits).SetValidator(new CreateEbitCommandValidator());
            RuleForEach(x => x.Opexes).SetValidator(new CreateOpexCommandValidator());
        }
    }
}
