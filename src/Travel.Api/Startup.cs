using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Travel.Api.Extensions;
using Travel.Infrastructure.DI.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Linq;
using Travel.Infrastructure.Storage;

namespace Travel.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAuthentication(o =>
            {
                o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddAzureAd(o => Configuration.Bind("AzureAd", o))
            .AddCookie();

            services.AddOptions();
            services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDb"));

            var builder = new ContainerBuilder();
            builder.Populate(services);
            Assembly[] assemblies = Assembly
                .GetEntryAssembly()
                .GetReferencedAssemblies()
                .Select(Assembly.Load)
                .ToArray();
            builder
                .AddMessageHandling(assemblies)
                .AddMongoDb(true, assemblies)
                .AddIdentity();

            return new AutofacServiceProvider(builder.Build());
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
