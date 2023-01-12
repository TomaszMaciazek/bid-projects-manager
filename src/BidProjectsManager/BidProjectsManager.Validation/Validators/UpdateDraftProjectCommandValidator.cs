using BidProjectsManager.DataLayer.Repositories;
using BidProjectsManager.Model.Commands;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BidProjectsManager.Validation.Validators
{
    public class UpdateDraftProjectCommandValidator : AbstractValidator<UpdateDraftProjectCommand>
    {
        public UpdateDraftProjectCommandValidator(IProjectRepository projectRepository, ICountryRepository countryRepository, ICurrencyRepository currencyRepository) {
            RuleFor(x => x.Id)
                .NotEmpty()
                .MustAsync(async (id, token) =>
                {
                    return await projectRepository.GetAll().AnyAsync(x => x.Id == id, cancellationToken: token);
                });

            RuleFor(x => x.Name).NotEmpty();

            RuleFor(x => x.CountryId)
                .NotEmpty()
                .MustAsync(async (id, token) =>
                {
                    return await countryRepository.GetAll().AnyAsync(x => x.Id == id, cancellationToken: token);
                });

            RuleFor(x => x.CurrencyId)
                .NotEmpty()
                .MustAsync(async (id, token) =>
                {
                    return await currencyRepository.GetAll().AnyAsync(x => x.Id == id, cancellationToken: token);
                });

            RuleFor(c => c)
                .NotEmpty()
                .MustAsync(async (cmd, cancelationToken) => {
                    return !await projectRepository.GetAll().AnyAsync(x => x.Name == cmd.Name && x.Id != cmd.Id, cancellationToken: cancelationToken);
                });

            RuleForEach(x => x.YearsToRemove).GreaterThanOrEqualTo(DateTime.Now.Year);
        }
    }
}
