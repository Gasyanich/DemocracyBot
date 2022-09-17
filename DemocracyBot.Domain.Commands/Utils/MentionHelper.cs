using DemocracyBot.DataAccess.Entities;

namespace DemocracyBot.Domain.Commands.Utils
{
    public static class MentionHelper
    {
        public static string GetMentionByUser(BotUser user)
        {
            return user.Username != null
                ? $"<a href=\"tg://user?id={user.Id}\">{user.Username}</a>"
                : $"<a href=\"tg://user?id={user.Id}\">Чмо без логина</a>";
        }
        
        public static string GetMentionByUser(long userId, string mentionName)
        {
            return $"<a href=\"tg://user?id={userId}\">{mentionName}</a>";
        }
    }
}