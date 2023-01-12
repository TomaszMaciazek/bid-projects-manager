using BidProjectsManager.DataLayer.Common;
using BidProjectsManager.Model.Commands;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BidProjectsManager.Validation.Validators
{
    public class CreateDraftProjectCommandValidator : AbstractValidator<CreateDraftProjectCommand>
    {
        public CreateDraftProjectCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MustAsync(async (name, cancelationToken) => {
                    return !await unitOfWork.ProjectRepository.GetAll().AnyAsync(x => x.Name == name, cancellationToken: cancelationToken);
                });

            RuleFor(c => c.CurrencyId)
                .NotEmpty()
                .MustAsync(async (currencyId, cancelationToken) =>
                {
                    return await unitOfWork.CurrencyRepository.GetAll().AnyAsync(x => x.Id == currencyId, cancellationToken: cancelationToken);
                });

            RuleFor(c => c.CountryId)
                .NotEmpty()
                .MustAsync(async (countryId, cancelationToken) =>
                {
                    return await unitOfWork.CountryRepository.GetAll().AnyAsync(x => x.Id == countryId, cancellationToken: cancelationToken);
                });

            RuleForEach(x => x.Capexes).SetValidator(new CreateCapexCommandValidator());
            RuleForEach(x => x.Ebits).SetValidator(new CreateEbitCommandValidator());
            RuleForEach(x => x.Opexes).SetValidator(new CreateOpexCommandValidator());
        }
    }
}
