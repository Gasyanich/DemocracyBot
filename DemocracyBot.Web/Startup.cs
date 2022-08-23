using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemocracyBot.DataAccess;
using DemocracyBot.Domain.Commands;
using DemocracyBot.Integration.Telegram;
using DemocracyBot.Integration.Weather;
using DemocracyBot.Integration.Weather.Dto;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace DemocracyBot.Web
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
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                })
                .AddFluentValidation();

            services.Configure<WeatherApiSettings>(Configuration.GetSection("WeatherApiSettings"));
            services.Configure<TelegramBotSettings>(Configuration.GetSection("TelegramBotSettings"));

            services.AddWeatherIntegration()
                .AddTelegramIntegration()
                .AddDataAccess(Configuration)
                .AddCommands();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }
    }
}