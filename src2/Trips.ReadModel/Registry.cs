using Microsoft.Extensions.DependencyInjection;
using Trips.Common;

namespace Trips.ReadModel
{
    public static class Registry
    {
        public static IServiceCollection AddTripsReadModelServices(this IServiceCollection services)
        {
            return services
                .AddAssemblyEventHandlers(typeof(Registry).Assembly);
        }
    }
}
