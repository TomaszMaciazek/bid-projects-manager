using BidProjectsManager.Model.Commands;
using FluentValidation;

namespace BidProjectsManager.Validation.Validators
{
    public class UpdateOpexCommandValidator : AbstractValidator<UpdateOpexCommand>
    {
        public UpdateOpexCommandValidator() { 
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
