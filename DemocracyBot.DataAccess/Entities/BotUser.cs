using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemocracyBot.DataAccess.Entities
{
    public class BotUser
    {
        /// <summary>
        ///     Id пользователя в тг
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        /// <summary>
        ///     Логин в тг
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// День рождения
        /// </summary>
        public DateTime BirthDate { get; set; }

        public long ChatId { get; set; }
        public Chat Chat { get; set; }

        public List<Meet> Meets { get; set; }
    }
}