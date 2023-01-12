using BidProjectsManager.DataLayer.Repositories;
using BidProjectsManager.Model.Commands;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BidProjectsManager.Validation.Validators
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator(ICountryRepository countryRepository, IUserRepository userRepository) {

            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Name).NotEmpty();

            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            RuleFor(x => x.Surname).NotEmpty();

            RuleFor(x => x)
            .MustAsync(async (cmd, cancelationToken) =>
            {
                var countriesIds = await countryRepository.GetAll().Select(y => y.Id).ToListAsync(cancellationToken: cancelationToken);
                return !await userRepository.GetAll().AnyAsync(x => x.Email == cmd.Email, cancellationToken: cancelationToken) &&
                    cmd.CountryIds.All(x => countriesIds.Contains(x));
            });
        }
    }
}
