namespace DemocracyBot.Integration.Telegram.TgMessages
{
    public abstract class TgMessage
    {
        public TgMessageChain Chain { get; set; }
        
        public bool IsPinMessage { get; set; }
    }
}