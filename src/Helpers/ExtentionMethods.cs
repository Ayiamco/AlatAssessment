using System;
using System.Net;
using System.Net.Http;

using AlatAssessment.DataAccess.Interfaces;
using AlatAssessment.DataAccess.Repositories;
using AlatAssessment.DataAccess.UnitOfWork;
using AlatAssessment.Services;
using AlatAssessment.Services.Implementation;
using AlatAssessment.Services.Interfaces;

using AutoMapper;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlatAssessment.Helpers
{
    public static class ExtentionMethods
    {
        public static IServiceCollection ConfigureAppSettings(this IServiceCollection serviceCollection,IConfiguration configuration)
        {
            AppSettings.ConnectionString = configuration["ConnectionString"];
            AppSettings.SubscriptionKey = configuration["SubscriptionKey"];
            AppSettings.WemaInternalUrl = configuration["WemaInternalUrl"];
            return serviceCollection;
        }

        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, AppUnitOfWork>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ILgaRepo, LgaRepository>();
            services.AddScoped<ICountryStateRepo, CountryStateRepository>();
            return services;
        }

        public static IServiceCollection ConfigureAppServices(this IServiceCollection services)
        {
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IWemaInternalHttpProxy, WemaInternalHttpProxyHttpProxy>();
            services.AddScoped<IPasswordManager, PasswordManager>();
            return services;
        }

        public static IServiceCollection ConfigureMapper(this IServiceCollection services)
        {
            //Add auto mapper service
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CustomerService.CustomerProfile());
            });
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }


        public static IServiceCollection ConfigureAppHttpClients(this IServiceCollection services )
        {
            services.AddHttpClient(WemaInternalHttpProxyHttpProxy.ClientProxy.WemaInternal, client =>
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", AppSettings.SubscriptionKey);
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip,
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
            }).SetHandlerLifetime(TimeSpan.FromMinutes(3));

            return services;
        }
    }
}
