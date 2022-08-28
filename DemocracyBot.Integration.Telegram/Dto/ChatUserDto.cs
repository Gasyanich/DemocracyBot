namespace DemocracyBot.Integration.Telegram.Dto
{
    public class ChatUserDto
    {
        public ChatUserDto(long userId, string userName)
        {
            UserId = userId;
            UserName = userName;
        }

        public long UserId { get; set; }

        public string UserName { get; set; }
    }
}