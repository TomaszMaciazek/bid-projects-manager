using BidProjectsManager.DataLayer.Repositories;
using BidProjectsManager.Model.Commands;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BidProjectsManager.Validation.Validators
{
    public class CreateCurrencyCommandValidator : AbstractValidator<CreateCurrencyCommand>
    {
        public CreateCurrencyCommandValidator(ICurrencyRepository currencyRepository) {
            RuleFor(cmd => cmd)
                .MustAsync(async (cmd, cancellationToken) =>
                {
                    return !await currencyRepository.GetAll().AnyAsync(x => x.Code.ToLower() == cmd.Code.ToLower() || x.Name.ToLower() == cmd.Name.ToLower(), cancellationToken: cancellationToken);
                });

            RuleFor(x => x.Code)
                .NotEmpty()
                .MaximumLength(3);

            RuleFor(x => x.Name)
                .NotEmpty();
        }
    }
}
