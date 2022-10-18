using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DemocracyBot.Integration.Telegram.TgMessages
{
    public static class TgMessageExtensions
    {
        public static TgMessageChain TextMessage(this TgMessageChain chain,
            string text)
        {
            chain.Add(new TgMessageText
            {
                Text = text,
                Chain = chain
            });

            return chain;
        }

        public static TgMessageChain StickerMessage(this TgMessageChain chain,
            string stickerId)
        {
            chain.Add(new TgMessageSticker
            {
                StickerId = stickerId,
                Chain = chain
            });

            return chain;
        }

        public static TgMessageChain PinMessage(this TgMessageChain chain)
        {
            var message = chain.Last();
            
            message.IsPinMessage = true;
            
            return message.Chain;
        }

        public static async Task Execute(this TelegramBotClient client, TgMessageChain chain)
        {
            var chatId = chain.ChatId;
            
            foreach (var tgMsg in chain)
            {
                var message = tgMsg switch
                {
                    TgMessageSticker sticker => await client.SendStickerAsync(chatId, new InputMedia(sticker.StickerId)),
                    TgMessageText text => await client.SendTextMessageAsync(chatId, text.Text),
                    _ => throw new ArgumentOutOfRangeException(nameof(tgMsg))
                };


                if (tgMsg.IsPinMessage)
                    await client.PinChatMessageAsync(chatId, message.MessageId);
            }
        }
    }
}