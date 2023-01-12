using BidProjectsManager.Model.Commands;
using FluentValidation;

namespace BidProjectsManager.Validation.Validators
{
    public class CreateEbitCommandValidator : AbstractValidator<CreateEbitCommand>
    {
        public CreateEbitCommandValidator()
        {
            RuleFor(x => x.Year).NotEmpty();
        }
    }
}
