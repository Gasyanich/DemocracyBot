using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DemocracyBot.DataAccess;
using DemocracyBot.DataAccess.Repository;
using DemocracyBot.DataAccess.Repository.Abstractions;
using DemocracyBot.Domain.Commands;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions.CommandsBase;
using DemocracyBot.Domain.Commands.Abstractions.Interactive;
using DemocracyBot.Domain.Commands.Services;
using DemocracyBot.Domain.Commands.Services.CommandFactory;
using DemocracyBot.Domain.Notification;
using DemocracyBot.Domain.Notification.Abstractions;
using DemocracyBot.Integration.Insult;
using DemocracyBot.Integration.Telegram;
using DemocracyBot.Integration.Telegram.Abstractions;
using DemocracyBot.Integration.Telegram.Dto;
using DemocracyBot.Integration.Weather;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using TL;
using WTelegram;

namespace DemocracyBot.LongPolling
{
    class Program
    {
        static async Task Main(string[] args)
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
                updateHandler: async (client, update, ct) =>
                {
                    var scope = provider.CreateScope();
                    var commandService = scope.ServiceProvider.GetRequiredService<ICommandService>();

                    await commandService.Handle(update);
                },
                pollingErrorHandler: (client, exception, arg3) =>
                {
                    var a = 5;
                },
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();
        }

        static string Config(string what)
        {
            switch (what)
            {
                case "api_id": return "2227391";
                case "api_hash": return "81f7429140163b499a496abdcc49db2e";
                case "phone_number": return "+79023217238";
                case "verification_code":
                    Console.Write("Code: ");
                    return Console.ReadLine();
                case "first_name": return "Игорь"; // if sign-up is required
                case "last_name": return "Игуменов"; // if sign-up is required
                case "password": return "secret!"; // if user has enabled 2FA
                default: return null; // let WTelegramClient decide the default config
            }
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddDbContext<DataContext>(builder =>
                builder.UseSqlServer(
                    "Server=217.28.223.127,17160;User Id=user_1b706;Password=Gi6=7P_rm3;Database=db_45475;"));

            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<ICommandFactoryService, CommandFactoryService>();
            services.AddScoped<ICommandService, CommandService>();
            services.AddSingleton<IStateManager, StateManager>();

            var commandTypes = typeof(CommandBase).Assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface && t.IsSubclassOf(typeof(CommandBase)));

            foreach (var commandType in commandTypes)
                services.AddScoped(commandType);

            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ITimeOfDayJobService, TimeOfDayJobService>();

            services.AddScoped(provider => new TelegramBotClient("5735722044:AAG9S5xRoA26_Jj1DLUYaeF2E6CL6tmfffg"));

            services.Configure<EvilInsultApiSettings>(settings => settings.ApiUrl = "https://evilinsult.com/");

            services.AddScoped<IEvilInsultService, EvilInsultService>();

            services.AddSingleton<IUserTelegramService, UserTelegramService>(_ =>
            {
                var client = new WTelegram.Client(Config);
                client.LoginUserIfNeeded().Wait();

                return new UserTelegramService
                {
                    Client = client
                };
            });
            services.AddScoped<IChatService, ChatService>();

            services.AddHttpClient<IWeatherService, WeatherService>();
            services.AddHttpClient<IEvilInsultService, EvilInsultService>();

            return services;
        }
    }
}