using Autofac;
using System;
using System.Reflection;
using Travel.Common.Cqrs;
using Travel.Infrastructure.Messaging;

namespace Travel.Infrastructure.DI
{
    public class CommandsModule : Autofac.Module
    {
        private Assembly[] assemblies;

        private CommandsModule() { }

        public static CommandsModule For(params Assembly[] assemblies)
            => new CommandsModule { assemblies = assemblies };

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(assemblies)
                .Where(x => x.IsAssignableTo<ICommandHandler>())
                .AsImplementedInterfaces();

            builder.Register<Func<Type, ICommandHandler>>(c =>
            {
                var ctx = c.Resolve<IComponentContext>();

                return t =>
                {
                    var handlerType = typeof(ICommandHandler<>).MakeGenericType(t);

                    return (ICommandHandler)ctx.Resolve(handlerType);
                };
            });

            builder.RegisterType<CommandDispatcher>()
                .AsImplementedInterfaces();
        }
    }
}
