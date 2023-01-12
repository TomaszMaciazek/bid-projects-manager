using BidProjectsManager.DataLayer.Repositories;
using BidProjectsManager.Model.Commands;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BidProjectsManager.Validation.Validators
{
    public class CreateCountryCommandValidator : AbstractValidator<CreateCountryCommand>
    {
        public CreateCountryCommandValidator(ICountryRepository countryRepository) {
            RuleFor(cmd => cmd)
                .MustAsync(async (cmd, cancellationToken) =>
                {
                    return !await countryRepository.GetAll().AnyAsync(x => x.Code.ToLower() == cmd.Code.ToLower() || x.Name.ToLower() == cmd.Name.ToLower(), cancellationToken: cancellationToken);
                });

            RuleFor(x => x.Code)
                .NotEmpty()
                .MaximumLength(3);

            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.CurrencyId)
                .NotEmpty();
        }
    }
}
