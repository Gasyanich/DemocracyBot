using DemocracyBot.Integration.Telegram.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace DemocracyBot.Integration.Telegram
{
    public static class Entry
    {
        public static IServiceCollection AddTelegramIntegration(this IServiceCollection services)
        {
            services.AddScoped(provider =>
            {
                var options = provider.GetRequiredService<IOptions<TelegramBotSettings>>();
                return new TelegramBotClient(options.Value.ApiKey);
            });

            services.AddSingleton<IUserTelegramService, UserTelegramService>();
            services.AddHostedService(provider => provider.GetService<IUserTelegramService>());
            services.AddScoped<IChatService, ChatService>();

            using var serviceProvider = services.BuildServiceProvider();
            var tgSettings = serviceProvider.GetRequiredService<IOptions<TelegramBotSettings>>().Value;
            var tgClient = serviceProvider.GetRequiredService<TelegramBotClient>();

            tgClient.SetWebhookAsync(tgSettings.WebhookUrl, allowedUpdates: new[] {UpdateType.Message, UpdateType.CallbackQuery}).Wait();

            return services;
        }
    }
}