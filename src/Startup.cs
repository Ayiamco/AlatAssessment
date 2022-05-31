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
using System.Reflection;
using System.Threading.Tasks;
using AlatAssessment.DataAccess.Contexts;
using AlatAssessment.DataAccess.Interfaces;
using AlatAssessment.DataAccess.Repositories;
using AlatAssessment.DataAccess.UnitOfWork;
using AlatAssessment.Helpers;
using AlatAssessment.Services;
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
           
            services.AddScoped<ModelStateValidator>();
            services.AddLogging(config =>
            {
                config.AddConsole();
                config.AddDebug();
            });

            services.ConfigureAppSettings(Configuration)
                .ConfigureRepositories()
                .ConfigureMapper()
                .ConfigureAppServices();
            
            
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
