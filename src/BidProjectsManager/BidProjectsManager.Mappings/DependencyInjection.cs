using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BidProjectsManager.Mappings
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}