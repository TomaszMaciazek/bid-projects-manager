using BidProjectsManager.DataLayer.Common;
using BidProjectsManager.DataLayer.Repositories;
using BidProjectsManager.Model.Commands;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BidProjectsManager.Validation.Validators
{
    public class SubmitProjectCommandValidator : AbstractValidator<SubmitProjectCommand>
    {
        public SubmitProjectCommandValidator(IUnitOfWork unitOfWork) {

            RuleFor(c => c)
                .NotEmpty()
                .MustAsync(async (cmd, cancelationToken) => {
                    return !await unitOfWork.ProjectRepository.GetAll().AnyAsync(x => x.Name == cmd.Name && x.Id != cmd.Id, cancellationToken: cancelationToken);
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
