using BidProjectsManager.DataLayer.Repositories;
using BidProjectsManager.Model.Commands;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BidProjectsManager.Validation.Validators
{
    public class CreateDraftProjectCommandValidator : AbstractValidator<CreateDraftProjectCommand>
    {
        public CreateDraftProjectCommandValidator(IProjectRepository projectRepository, ICountryRepository countryRepository, ICurrencyRepository currencyRepository)
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
                    return await currencyRepository.GetAll().AnyAsync(x => x.Id == currencyId, cancellationToken: cancelationToken);
                });

            RuleFor(c => c.CountryId)
                .NotEmpty()
                .MustAsync(async (countryId, cancelationToken) =>
                {
                    return await countryRepository.GetAll().AnyAsync(x => x.Id == countryId, cancellationToken: cancelationToken);
                });

            RuleForEach(x => x.Capexes).SetValidator(new CreateCapexCommandValidator());
            RuleForEach(x => x.Ebits).SetValidator(new CreateEbitCommandValidator());
            RuleForEach(x => x.Opexes).SetValidator(new CreateOpexCommandValidator());
        }
    }
}
