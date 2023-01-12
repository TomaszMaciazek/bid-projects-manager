using BidProjectsManager.Model.Commands;
using FluentValidation;

namespace BidProjectsManager.Validation.Validators
{
    public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator() { 
            RuleFor(x => x.ProjectId).NotEmpty();
            RuleFor(x => x.Content).NotEmpty();
        }
    }
}
