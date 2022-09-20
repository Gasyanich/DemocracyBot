using DemocracyBot.DataAccess;
using DemocracyBot.Domain.Commands;
using DemocracyBot.Domain.Notification;
using DemocracyBot.Integration.Insult;
using DemocracyBot.Integration.Telegram;
using DemocracyBot.Integration.Weather;
using DemocracyBot.Integration.Weather.Dto;
using DemocracyBot.Web.Hangfire;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
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
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .AddFluentValidation();

            services
                .AddHangfire(configuration => configuration.UseMemoryStorage())
                .AddHangfireServer();

            services.Configure<WeatherApiSettings>(Configuration.GetSection("WeatherApiSettings"));
            services.Configure<TelegramBotSettings>(Configuration.GetSection("TelegramBotSettings"));
            services.Configure<EvilInsultApiSettings>(Configuration.GetSection("EvilInsultApiSettings"));

            services.AddWeatherIntegration()
                .AddTelegramIntegration()
                .AddInsultIntegration()
                .AddDataAccess(Configuration)
                .AddCommands()
                .AddNotifications();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseRouting();

            app.UseHangfireDashboard();

            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });

            HangfireJobRegisterHelper.RegisterJobs(Configuration);
        }
    }
}