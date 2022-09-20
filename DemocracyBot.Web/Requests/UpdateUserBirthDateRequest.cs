using System;

namespace DemocracyBot.Web.Requests
{
    public class UpdateUserBirthDateRequest
    {
        public long ChatId { get; set; }
        public long UserId { get; set; }

        public DateTime BirthDate { get; set; }
    }
}