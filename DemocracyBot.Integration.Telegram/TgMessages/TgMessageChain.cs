using System.Collections.Generic;

namespace DemocracyBot.Integration.Telegram.TgMessages
{
    public class TgMessageChain : List<TgMessage>
    {
        public static TgMessageChain Create(long chatId)
        {
            return new TgMessageChain
            {
                ChatId = chatId
            };
        }

        public long ChatId { get; set; }
    }
}