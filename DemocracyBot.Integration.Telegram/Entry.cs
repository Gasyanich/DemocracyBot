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

            using var serviceProvider = services.BuildServiceProvider();
            var tgSettings = serviceProvider.GetRequiredService<IOptions<TelegramBotSettings>>().Value;
            var tgClient = serviceProvider.GetRequiredService<TelegramBotClient>();

            tgClient.SetWebhookAsync(tgSettings.WebhookUrl, allowedUpdates: new[] {UpdateType.Message}).Wait();

            return services;
        }
    }
}