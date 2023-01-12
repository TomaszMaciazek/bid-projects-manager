using BidProjectsManager.Model.Commands;
using FluentValidation;

namespace BidProjectsManager.Validation.Validators
{
    public class UpdateCapexCommandValidator : AbstractValidator<UpdateCapexCommand>
    {
        public UpdateCapexCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
