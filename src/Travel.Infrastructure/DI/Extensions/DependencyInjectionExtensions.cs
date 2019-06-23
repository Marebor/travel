using Autofac;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using Travel.Common.Auth;
using Travel.Infrastructure.Auth;

namespace Travel.Infrastructure.DI.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static ContainerBuilder AddMessageHandling(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            builder
                .RegisterModule(CommandsModule.For(assemblies))
                .RegisterModule(EventsModule.For(assemblies))
                .RegisterModule(QueryModule.For(assemblies));

            return builder;
        }

        public static ContainerBuilder AddMongoDb(this ContainerBuilder builder, bool seedDatabase, params Assembly[] assemblies)
        {
            builder.RegisterModule(StorageModule.For(assemblies, seedDatabase));

            return builder;
        }

        public static ContainerBuilder AddIdentity(this ContainerBuilder builder)
        {
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            builder.RegisterType<IdentityProvider>().As<IIdentityProvider>();

            return builder;
        }
    }
}
