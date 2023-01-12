using BidProjectsManager.Model.Commands;
using FluentValidation;

namespace BidProjectsManager.Validation.Validators
{
    public class UpdateEbitCommandValidator : AbstractValidator<UpdateEbitCommand>
    {
        public UpdateEbitCommandValidator() {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
