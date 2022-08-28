using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemocracyBot.DataAccess.Entities
{
    public class BotUser
    {
        /// <summary>
        ///     Id пользователя
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        /// <summary>
        ///     Логин в тг
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     Текущая репутация
        /// </summary>
        public int ReputationScore { get; set; }

        /// <summary>
        ///     Кол-во очков репутации для голосования
        /// </summary>
        public int AvailableReputationScore { get; set; }

        /// <summary>
        ///     Статус (пока герой/не герой)
        /// </summary>
        public UserStatus Status { get; set; }

        /// <summary>
        /// Сколько раз пытался наебать правосудие голосованием с 0 очков или увеличить/уменьшить реп на недостающее число очков
        /// </summary>
        public int ReputationVoteErrorCount { get; set; }

        public long ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}