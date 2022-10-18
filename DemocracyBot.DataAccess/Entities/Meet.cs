using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DemocracyBot.DataAccess.Entities
{
    public class Meet
    {
        [Key] public long Id { get; set; }

        public DateTimeOffset DateTime { get; set; }

        public long ChatId { get; set; }
        public Chat Chat { get; set; }

        public List<BotUser> Users { get; set; }
    }
}