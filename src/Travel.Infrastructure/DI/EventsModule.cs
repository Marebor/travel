using Autofac;
using System;
using System.Collections.Generic;
using System.Reflection;
using Travel.Common.Cqrs;
using Travel.Infrastructure.Messaging;

namespace Travel.Infrastructure.DI
{
    public class EventsModule : Autofac.Module
    {
        private Assembly[] assemblies;

        private EventsModule() { }

        public static EventsModule For(params Assembly[] assemblies)
            => new EventsModule { assemblies = assemblies };

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(assemblies)
                .Where(x => x.IsAssignableTo<IEventHandler>())
                .AsImplementedInterfaces();
            
            builder.Register<Func<Type, IEnumerable<IEventHandler>>>(c =>
            {
                var ctx = c.Resolve<IComponentContext>();
                return t =>
                {
                    var handlerType = typeof(IEventHandler<>).MakeGenericType(t);
                    var handlersCollectionType = typeof(IEnumerable<>).MakeGenericType(handlerType);

                    return (IEnumerable<IEventHandler>)ctx.Resolve(handlersCollectionType);
                };
            });

            builder.RegisterType<EventPublisher>()
                .AsImplementedInterfaces();
        }
    }
}
