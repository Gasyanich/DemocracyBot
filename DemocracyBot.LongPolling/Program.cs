using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DemocracyBot.DataAccess;
using DemocracyBot.DataAccess.Repository;
using DemocracyBot.DataAccess.Repository.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions.CommandsBase;
using DemocracyBot.Domain.Commands.Abstractions.Interactive;
using DemocracyBot.Domain.Commands.Services;
using DemocracyBot.Domain.Commands.Services.CommandFactory;
using DemocracyBot.Domain.Notification;
using DemocracyBot.Domain.Notification.Abstractions;
using DemocracyBot.Integration.Weather;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace DemocracyBot.LongPolling
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var services = ConfigureServices();
            var provider = services.BuildServiceProvider();

            var botClient = provider.GetRequiredService<TelegramBotClient>();

            using var cts = new CancellationTokenSource();

            await botClient.DeleteWebhookAsync();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
            };
            botClient.StartReceiving(
                async (client, update, ct) =>
                {
                    var scope = provider.CreateScope();
                    var commandService = scope.ServiceProvider.GetRequiredService<ICommandService>();

                    await commandService.Handle(update);
                },
                (client, exception, arg3) =>
                {
                    var a = 5;
                },
                receiverOptions,
                cts.Token
            );

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddDbContext<DataContext>(builder =>
                builder.UseSqlServer(
                    "Server=217.28.223.127,17160;User Id=user_1b706;Password=Gi6=7P_rm3;Database=db_45475;"));

            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IMeetRepository, MeetRepository>();

            services.AddScoped<ICommandFactoryService, CommandFactoryService>();
            services.AddScoped<ICommandService, CommandService>();
            services.AddSingleton<IStateManager, StateManager>();
            services.AddSingleton<IRestrictService, RestrictService>();

            var commandTypes = typeof(CommandBase).Assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface && t.IsSubclassOf(typeof(CommandBase)));

            foreach (var commandType in commandTypes)
                services.AddScoped(commandType);

            services.AddScoped<ITimeOfDayJobService, TimeOfDayJobService>();

            services.AddScoped(provider => new TelegramBotClient("5735722044:AAG9S5xRoA26_Jj1DLUYaeF2E6CL6tmfffg"));

            services.AddHttpClient<IWeatherService, WeatherService>();
            services.AddHangfire(configuration => configuration.UseMemoryStorage());

            return services;
        }
    }
}