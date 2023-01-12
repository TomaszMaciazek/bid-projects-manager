using BidProjectsManager.Logic.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BidProjectsManager.Logic
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddLogic(this IServiceCollection services)
        {
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IProjectService, ProjectService>();
            return services;
        }
    }
}