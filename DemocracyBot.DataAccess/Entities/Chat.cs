using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemocracyBot.DataAccess.Entities
{
    public class Chat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        public bool IsNotificationsActivated { get; set; }

        public List<BotUser> Users { get; set; }
        public List<Meet> Meets { get; set; }
    }
}