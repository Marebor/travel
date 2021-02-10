using Microsoft.Extensions.DependencyInjection;
using System;

namespace Trips.Domain.Repositories
{
    internal sealed class EntityBuildersFactory : IEntityBuildersFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityBuildersFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        IEntityBuilder IEntityBuildersFactory.GetBuilder(Type entityType, Type eventType)
        {
            var applierType = typeof(IEntityBuilder<,>).MakeGenericType(entityType, eventType);

            return (IEntityBuilder)_serviceProvider.GetRequiredService(applierType);
        }
    }
}
