using BidProjectsManager.DataLayer.Repositories;
using BidProjectsManager.Model.Commands;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BidProjectsManager.Validation.Validators
{
    public class SubmitProjectCommandValidator : AbstractValidator<SubmitProjectCommand>
    {
        public SubmitProjectCommandValidator(IProjectRepository projectRepository, ICountryRepository countryRepository, ICurrencyRepository currencyRepository) {

            RuleFor(c => c)
                .NotEmpty()
                .MustAsync(async (cmd, cancelationToken) => {
                    return !await projectRepository.GetAll().AnyAsync(x => x.Name == cmd.Name && x.Id != cmd.Id, cancellationToken: cancelationToken);
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
            RuleFor(c => c.Probability).NotEmpty();
            RuleFor(c => c.BidEstimatedOperationEnd).NotEmpty();
            RuleFor(c => c.BidOperationStart).NotEmpty();
            RuleFor(c => c.BidStatus).NotEmpty();
            RuleFor(c => c.CountryId).NotEmpty();
            RuleFor((c => c.Description)).NotEmpty();
            RuleFor(c => c.LifetimeInKilometers).NotEmpty();
            RuleFor(c => c.Priority).NotEmpty();
            RuleFor(c => c.Stage).NotEmpty();

            RuleForEach(x => x.NewCapexes).SetValidator(new CreateCapexCommandValidator());
            RuleForEach(x => x.NewEbits).SetValidator(new CreateEbitCommandValidator());
            RuleForEach(x => x.NewOpexes).SetValidator(new CreateOpexCommandValidator());

            RuleForEach(x => x.Capexes).SetValidator(new UpdateCapexCommandValidator());
            RuleForEach(x => x.Ebits).SetValidator(new UpdateEbitCommandValidator());
            RuleForEach(x => x.Opexes).SetValidator(new UpdateOpexCommandValidator());

            RuleForEach(x => x.YearsToRemove).GreaterThanOrEqualTo(DateTime.Now.Year);
        }
    }
}
