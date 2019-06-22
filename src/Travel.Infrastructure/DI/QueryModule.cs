﻿using Autofac;
using System;
using System.Reflection;
using Travel.Common.Cqrs;
using Travel.Infrastructure.Messaging;

namespace Travel.Infrastructure.DI
{
    public class QueryModule : Autofac.Module
    {
        private Assembly[] assemblies;

        private QueryModule() { }

        public static QueryModule For(params Assembly[] assemblies)
            => new QueryModule { assemblies = assemblies };

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(assemblies)
                .Where(x => x.IsAssignableTo<IQueryHandler>())
                .AsImplementedInterfaces();

            builder.Register<Func<Type, Type, IQueryHandler>>(c =>
            {
                var ctx = c.Resolve<IComponentContext>();

                return (tQuery, tResult) =>
                {
                    var handlerType = typeof(IQueryHandler<,>).MakeGenericType(tQuery, tResult);

                    return (IQueryHandler)ctx.Resolve(handlerType);
                };
            });

            builder.RegisterType<QueryExecutor>()
                .AsImplementedInterfaces();
        }
    }
}
