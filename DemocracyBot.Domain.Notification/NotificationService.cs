using System.Threading.Tasks;
using DemocracyBot.Domain.Notification.Abstractions;
using DemocracyBot.Domain.Notification.Dto;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DemocracyBot.Domain.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly TelegramBotClient _botClient;

        public NotificationService(TelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task SendNotificationToChat(NotificationMessageDto messageDto)
        {
            var chatId = messageDto.ChatId;

            await SendStickerIfNotNull(chatId, messageDto.BeforeMessageStickerFileId);

            var message = await _botClient.SendTextMessageAsync(chatId, messageDto.MessageText, ParseMode.Html);

            if (messageDto.IsPinMessage)
                await _botClient.PinChatMessageAsync(chatId, message.MessageId, false);

            foreach (var stickerFIleId in messageDto.StickerFIleIds)
            {
                await SendStickerIfNotNull(chatId, stickerFIleId);
                await Task.Delay(200);
            }

            await SendStickerIfNotNull(chatId, messageDto.AfterMessageStickerFileId);
        }

        private async Task SendStickerIfNotNull(long chatId, string fileId)
        {
            if (fileId != null)
                await _botClient.SendStickerAsync(chatId, new InputMedia(fileId));
        }
    }
}