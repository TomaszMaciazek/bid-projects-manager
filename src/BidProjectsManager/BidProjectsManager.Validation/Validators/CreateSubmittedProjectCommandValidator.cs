using BidProjectsManager.DataLayer.Common;
using BidProjectsManager.Model.Commands;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BidProjectsManager.Validation.Validators
{
    public class CreateSubmittedProjectCommandValidator : AbstractValidator<CreateSubmittedProjectCommand>
    {
        public CreateSubmittedProjectCommandValidator(IUnitOfWork unitOfWork)
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

            RuleFor(c => c.Probability).NotEmpty();
            RuleFor(c => c.BidEstimatedOperationEnd).NotEmpty();
            RuleFor(c => c.BidOperationStart).NotEmpty();
            RuleFor(c => c.Status).NotEmpty();
            RuleFor(c => c.CountryId).NotEmpty();
            RuleFor((c => c.Description)).NotEmpty();
            RuleFor(c => c.Priority).NotEmpty();

            RuleForEach(x => x.Capexes).SetValidator(new CreateCapexCommandValidator());
            RuleForEach(x => x.Ebits).SetValidator(new CreateEbitCommandValidator());
            RuleForEach(x => x.Opexes).SetValidator(new CreateOpexCommandValidator());
        }
    }
}
