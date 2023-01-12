using BidProjectsManager.DataLayer.Common;
using BidProjectsManager.DataLayer.Repositories;
using BidProjectsManager.Model.Commands;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidProjectsManager.Validation.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator(IUnitOfWork unitOfWork) {
            RuleFor(x => x.Name).NotEmpty();

            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            RuleFor(x => x.Surname).NotEmpty();

            RuleFor(x => x)
            .MustAsync(async (cmd, cancelationToken) =>
            {
                var countriesIds = await unitOfWork.CountryRepository.GetAll().Select(y => y.Id).ToListAsync(cancellationToken: cancelationToken);
                return !await unitOfWork.UserRepository.GetAll().AnyAsync(x => x.Email == cmd.Email, cancellationToken: cancelationToken) && 
                    cmd.CountryIds.All(x => countriesIds.Contains(x));
            });
        }
    }
}
