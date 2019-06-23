using Autofac;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Authentication;
using Travel.Common.Cqrs;
using Travel.Common.Model;
using Travel.Infrastructure.Storage;

namespace Travel.Infrastructure.DI
{
    public class StorageModule : Autofac.Module
    {
        private bool seedDatabase;
        private Assembly[] assemblies;

        private StorageModule() { }

        public static StorageModule For(Assembly[] assemblies, bool seed)
            => new StorageModule { assemblies = assemblies, seedDatabase = seed };

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            MapAllEvents();
            ConventionRegistry.Register("MyConventions", new MyConventions(), _ => true);

            builder.Register<IMongoClient>(c =>
            {
                var ctx = c.Resolve<IComponentContext>();
                var options = ctx.Resolve<IOptions<MongoDbSettings>>();

                MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(options.Value.ConnectionString));
                settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

                return new MongoClient(settings);
            })
            .SingleInstance();

            builder.Register<IMongoDatabase>(c =>
            {
                var ctx = c.Resolve<IComponentContext>();

                var options = ctx.Resolve<IOptions<MongoDbSettings>>();
                var client = ctx.Resolve<IMongoClient>();

                var db = client.GetDatabase(options.Value.Database);

                if (seedDatabase)
                {
                    db.SeedDatabase();
                }

                return db;
            });

            assemblies
               .SelectMany(x => x.GetTypes())
               .Where(x => typeof(Aggregate).IsAssignableFrom(x))
               .ToList()
               .ForEach(x =>
               {
                   var storeType = typeof(AggregateStore<>).MakeGenericType(x);
                   builder.RegisterType(storeType).AsImplementedInterfaces();
               });
            
            assemblies
               .SelectMany(x => x.GetTypes())
               .SelectMany(x => x.GetInterfaces())
               .Where(x => typeof(IQuery).IsAssignableFrom(x))
               .SelectMany(x => x.GetGenericArguments())
               .Select(x => typeof(IEnumerable).IsAssignableFrom(x) ? x.GetGenericArguments().First() : x)
               .Distinct()
               .ToList()
               .ForEach(x =>
               {
                   var storeType = typeof(QueryableStore<>).MakeGenericType(x);
                   builder.RegisterType(storeType).AsImplementedInterfaces();
               });
        }
        
        private void MapAllEvents()
        {
            var eventTypes = assemblies
                .SelectMany(x => x.GetTypes())
                .Where(t => typeof(IEvent).IsAssignableFrom(t))
                .Where(t => t.IsClass);

            foreach (var type in eventTypes)
            {
                var registrationMethod = typeof(BsonClassMap).GetMethods()
                    .Where(m => m.Name == nameof(BsonClassMap.RegisterClassMap))
                    .Where(m => m.GetParameters().Length == 0)
                    .First()
                    .MakeGenericMethod(type);

                registrationMethod.Invoke(null, new object[] { });
            }
        }

        private class MyConventions : IConventionPack
        {
            public IEnumerable<IConvention> Conventions
                => new List<IConvention>
                {
                    new IgnoreExtraElementsConvention(true),
                    new EnumRepresentationConvention(BsonType.String),
                    new CamelCaseElementNameConvention(),
                };
        }
    }
}