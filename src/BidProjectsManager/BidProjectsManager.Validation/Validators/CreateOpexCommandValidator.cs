using BidProjectsManager.Model.Commands;
using FluentValidation;

namespace BidProjectsManager.Validation.Validators
{
    public class CreateOpexCommandValidator : AbstractValidator<CreateOpexCommand>
    {
        public CreateOpexCommandValidator() {
            RuleFor(x => x.Year).NotEmpty();
        }
    }
}
