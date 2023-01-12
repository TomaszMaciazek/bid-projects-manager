using BidProjectsManager.DataLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BidProjectsManager.DataLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient, ServiceLifetime.Singleton);

            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IProjectCommentRepository, ProjectCommentRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICapexRepository, CapexRepository>();
            services.AddScoped<IEbitRepository, EbitRepository>();
            services.AddScoped<IOpexRepository, OpexRepository>();
            services.AddTransient<IDataInitializer, DataInitializer>();
            return services;
        }
    }
}