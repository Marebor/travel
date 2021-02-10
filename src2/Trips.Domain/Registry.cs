using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Trips.Common;
using Trips.Domain.Commands;
using Trips.Domain.Commands.Handlers;
using Trips.Domain.Events;
using Trips.Domain.Repositories;

namespace Trips.Domain
{
    public static class Registry
    {
        public static IServiceCollection AddTripsDomainServices(this IServiceCollection services)
        {
            return services
                .AddCommandHandlers()
                .AddEventAppliers()
                .AddCommandHandlersFactory()
                .AddEventHandlersFactory()
                //.AddEventAppliersFactory()
                .AddScoped<ICommandDispatcher, InMemoryCommandDispatcher>()
                .AddScoped<IEventPublisher, InMemoryEventPublisher>()
                .AddScoped<IEntityRepository, EventBasedEntityRepository>()
                .AddScoped<IEntityBuildersFactory, EntityBuildersFactory>()
                .AddScoped<IEntityCache, EntityCache>()
                    .AddMemoryCache();
        }

        private static IServiceCollection AddCommandHandlers(this IServiceCollection services)
        {
            var handlers = typeof(Registry).Assembly
                .GetTypes()
                .Where(type => type.IsClass && typeof(ICommandHandler).IsAssignableFrom(type));

            foreach (var handler in handlers)
            {
                var commandTypes = handler
                    .GetInterfaces()
                    .Where(i => i.IsGenericType && i.Name.StartsWith(nameof(ICommandHandler)))
                    .Select(i => i.GetGenericArguments()[0]);

                foreach (var commandType in commandTypes)
                {
                    var handlerInterface = typeof(ICommandHandler<>).MakeGenericType(commandType);

                    services.AddScoped(handlerInterface, handler);
                }
            }

            return services;
        }

        private static IServiceCollection AddEventAppliers(this IServiceCollection services)
        {
            var appliers = typeof(Registry).Assembly
                .GetTypes()
                .Where(type => type.IsClass && typeof(IEntityBuilder).IsAssignableFrom(type));

            foreach (var applier in appliers)
            {
                var applierGenericArguments = applier
                    .GetInterfaces()
                    .Where(i => i.IsGenericType && i.Name.StartsWith(nameof(IEntityBuilder)))
                    .Select(i => i.GetGenericArguments());

                foreach (var genericArguments in applierGenericArguments)
                {
                    var applierInterface = typeof(IEntityBuilder<,>).MakeGenericType(genericArguments);

                    services.AddScoped(applierInterface, applier);
                }
            }

            return services;
        }

        private static IServiceCollection AddCommandHandlersFactory(this IServiceCollection services)
        {
            return services.AddScoped<Func<Type, ICommandHandler>>(serviceProvider =>
            {
                return type =>
                {
                    var handlerType = typeof(ICommandHandler<>).MakeGenericType(type);

                    return (ICommandHandler)serviceProvider.GetRequiredService(handlerType);
                };
            });
        }

        private static IServiceCollection AddEventHandlersFactory(this IServiceCollection services)
        {
            return services.AddScoped<Func<Type, IEnumerable<IEventHandler>>>(serviceProvider =>
            {
                return type =>
                {
                    var handlerType = typeof(IEventHandler<>).MakeGenericType(type);

                    return (IEnumerable<IEventHandler>)serviceProvider.GetServices(handlerType);
                };
            });
        }

        //private static IServiceCollection AddEventAppliersFactory(this IServiceCollection services)
        //{
        //    return services.AddScoped<Func<Type, IEventApplier>>(serviceProvider =>
        //    {
        //        return type =>
        //        {
        //            var applierType = typeof(IEventApplier<>).MakeGenericType(type);

        //            return (IEventApplier)serviceProvider.GetRequiredService(applierType);
        //        };
        //    });
        //}
    }
}
