using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace Trips.Common
{
    public static class EventHandlersRegistry
    {
        public static IServiceCollection AddAssemblyEventHandlers(this IServiceCollection services, Assembly assembly)
        {
            var handlers = assembly
                .GetTypes()
                .Where(type => type.IsClass && typeof(IEventHandler).IsAssignableFrom(type));

            foreach (var handler in handlers)
            {
                var eventTypes = handler
                    .GetInterfaces()
                    .Where(i => i.IsGenericType && i.Name.StartsWith(nameof(IEventHandler)))
                    .Select(i => i.GetGenericArguments()[0]);

                foreach (var eventType in eventTypes)
                {
                    var handlerInterface = typeof(IEventHandler<>).MakeGenericType(eventType);

                    services.AddScoped(handlerInterface, handler);
                }
            }

            return services;
        }
    }
}
