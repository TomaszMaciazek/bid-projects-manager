using BidProjectsManager.Model.Commands;
using FluentValidation;

namespace BidProjectsManager.Validation.Validators
{
    public class CreateCapexCommandValidator : AbstractValidator<CreateCapexCommand>
    {
        public CreateCapexCommandValidator()
        {
            RuleFor(x => x.Year).NotEmpty();
        }
    }
}
