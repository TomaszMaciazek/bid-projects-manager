using BidProjectsManager.DataLayer.Common;
using BidProjectsManager.DataLayer.Repositories;
using BidProjectsManager.Model.Commands;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BidProjectsManager.Validation.Validators
{
    public class UpdateDraftProjectCommandValidator : AbstractValidator<UpdateDraftProjectCommand>
    {
        public UpdateDraftProjectCommandValidator(IUnitOfWork unitOfWork) {
            RuleFor(x => x.Id)
                .NotEmpty()
                .MustAsync(async (id, token) =>
                {
                    return await unitOfWork.ProjectRepository.GetAll().AnyAsync(x => x.Id == id, cancellationToken: token);
                });

            RuleFor(x => x.Name).NotEmpty();

            RuleFor(x => x.CountryId)
                .NotEmpty()
                .MustAsync(async (id, token) =>
                {
                    return await unitOfWork.CountryRepository.GetAll().AnyAsync(x => x.Id == id, cancellationToken: token);
                });

            RuleFor(x => x.CurrencyId)
                .NotEmpty()
                .MustAsync(async (id, token) =>
                {
                    return await unitOfWork.CurrencyRepository.GetAll().AnyAsync(x => x.Id == id, cancellationToken: token);
                });

            RuleFor(c => c)
                .NotEmpty()
                .MustAsync(async (cmd, cancelationToken) => {
                    return !await unitOfWork.ProjectRepository.GetAll().AnyAsync(x => x.Name == cmd.Name && x.Id != cmd.Id, cancellationToken: cancelationToken);
                });

            RuleForEach(x => x.YearsToRemove).GreaterThanOrEqualTo(DateTime.Now.Year);
        }
    }
}
