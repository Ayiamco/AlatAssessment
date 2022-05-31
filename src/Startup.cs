using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AlatAssessment.DataAccess.Contexts;
using AlatAssessment.DataAccess.Interfaces;
using AlatAssessment.DataAccess.Repositories;
using AlatAssessment.DataAccess.UnitOfWork;
using AlatAssessment.Helpers;
using AlatAssessment.Services.Implementation;
using AlatAssessment.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;


namespace AlatAssessment
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AppSettings.ConnectionString = Configuration["ConnectionString"];
            AppSettings.SubscriptionKey = Configuration["SubscriptionKey"];
            AppSettings.WemaInternalUrl = Configuration["WemaInternalUrl"];
            services.AddScoped<ModelStateValidator>();
            services.AddLogging(config =>
            {
                config.AddConsole();
                config.AddDebug();
            });
            services.AddScoped<IUnitOfWork, AppUnitOfWork>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ILgaRepo, LgaRepository>();
            services.AddScoped<ICountryStateRepo, CountryStateRepository>();
            services.AddScoped<INotificationService,NotificationService>();
            services.AddScoped<ICustomerService,CustomerService >();
            services.AddScoped<IWemaInternal,WemaInternal>();

            services.AddHttpClient(WemaInternal.ClientProxy.WemaInternal, client =>
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", AppSettings.SubscriptionKey);
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip,
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
            }).SetHandlerLifetime(TimeSpan.FromMinutes(300));


            //Add auto mapper service
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CustomerService.CustomerProfile());
            });
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);


            services.AddControllers();
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(AppSettings.ConnectionString));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AlatAssessment", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            await Helpers.CsvHelper.EnsureSeedDataPopulated(app);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AlatAssessment v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
