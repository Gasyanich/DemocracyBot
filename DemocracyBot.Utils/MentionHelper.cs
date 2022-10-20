namespace DemocracyBot.Utils
{
    public static class MentionHelper
    {
        public static string GetMentionByUser(long userId, string mentionName)
        {
            return $"<a href=\"tg://user?id={userId}\">{mentionName}</a>";
        }
    }
}