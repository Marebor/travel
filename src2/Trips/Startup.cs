using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Trips.Common;
using Trips.Domain;
using Trips.Domain.Authorization;
using Trips.Domain.Repositories;
using Trips.Infrastructure;
using Trips.ReadModel;

namespace Trips
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private bool UseAzureAd => Configuration.GetValue<bool>("UseAzureAd");

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllersWithViews()
                .AddNewtonsoftJson()
                .Services
                .AddSpaStaticFiles(configuration =>
                {
                    configuration.RootPath = "ClientApp/dist";
                });
            services
                .AddSingleton<ICreatedEntityIdResolver, CreatedEntityIdResolver>()
                    .AddMemoryCache()
                .AddScoped<IEventRepository, EfCoreDomainRepository>()
                    .AddDbContext<TripsDomainContext>(options =>
                    {
                        options.UseSqlServer("name=ConnectionStrings:TripsDomainConnection");
                    })
                .AddScoped<ReadModel.Repositories.ICustomerRepository, EfCoreReadModelRepository>()
                .AddScoped<ReadModel.Repositories.ITripRepository, EfCoreReadModelRepository>()
                    .AddDbContext<TripsReadModelContext>(options =>
                    {
                        options.UseSqlServer("name=ConnectionStrings:TripsReadModelConnection");
                    })
                .AddHttpClient<IDestinationRepository, RestCountriesComDestinationRepository>(client =>
                {
                    client.BaseAddress = new Uri("https://restcountries.com");
                })
                .Services
                .AddAssemblyEventHandlers(typeof(Startup).Assembly)
                .AddTripsDomainServices()
                .AddTripsReadModelServices();

            if (UseAzureAd)
            {
                services.AddAuthentication(o =>
                {
                    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddAzureAd(o => Configuration.Bind("AzureAd", o))
                .AddCookie()
                .Services
                .AddScoped<IUserDataProvider, HttpContextUserDataProvider>()
                    .AddHttpContextAccessor();
            }
            else
            {
                services.AddScoped<IUserDataProvider, FakeUserDataProvider>();
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            if (UseAzureAd)
            {
                app.UseAuthentication();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
