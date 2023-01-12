using BidProjectsManager.Model.Commands;
using BidProjectsManager.Validation.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BidProjectsManager.Validation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddValidation(this IServiceCollection services) {
            services.AddScoped<IValidator<CreateCountryCommand>, CreateCountryCommandValidator>();
            services.AddScoped<IValidator<UpdateCountryCommand>, UpdateCountryCommandValidator>();
            services.AddScoped<IValidator<CreateCurrencyCommand>, CreateCurrencyCommandValidator>();
            services.AddScoped<IValidator<UpdateCurrencyCommand>, UpdateCurrencyCommandValidator>();
            services.AddScoped<IValidator<CreateCommentCommand>, CreateCommentCommandValidator>();
            services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
            services.AddScoped<IValidator<UpdateUserCommand>, UpdateUserCommandValidator>();
            services.AddScoped<IValidator<CreateCapexCommand>, CreateCapexCommandValidator>();
            services.AddScoped<IValidator<CreateEbitCommand>, CreateEbitCommandValidator>();
            services.AddScoped<IValidator<CreateOpexCommand>, CreateOpexCommandValidator>();
            services.AddScoped<IValidator<UpdateCapexCommand>, UpdateCapexCommandValidator>();
            services.AddScoped<IValidator<UpdateEbitCommand>, UpdateEbitCommandValidator>();
            services.AddScoped<IValidator<UpdateOpexCommand>, UpdateOpexCommandValidator>();
            services.AddScoped<IValidator<CreateDraftProjectCommand>, CreateDraftProjectCommandValidator>();
            services.AddScoped<IValidator<CreateSubmittedProjectCommand>, CreateSubmittedProjectCommandValidator>();
            services.AddScoped<IValidator<UpdateDraftProjectCommand>, UpdateDraftProjectCommandValidator>();
            services.AddScoped<IValidator<SubmitProjectCommand>, SubmitProjectCommandValidator>();
            return services;
        }
    }
}