using Microsoft.Extensions.DependencyInjection;

namespace BidProjectsManager.DataLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataLayer(this IServiceCollection services)
        {
            return services;
        }
    }
}