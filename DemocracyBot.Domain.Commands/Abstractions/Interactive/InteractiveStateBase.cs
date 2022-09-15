using System;

namespace DemocracyBot.Domain.Commands.Abstractions.Interactive
{
    public abstract class InteractiveStateBase<TStep> : InteractiveStateBase where TStep : Enum
    {
        /// <summary>
        /// Текущий шаг
        /// </summary>
        public TStep CurrentStep { get; set; }

        public bool IsLastStep => CurrentStep.Equals(default(TStep));
    }

    public abstract class InteractiveStateBase
    {
        /// <summary>
        /// Id пользователя в интерактивной команде
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Последнее сообщение, которое отправил бот для уточняющего вопроса/ввода текста
        /// </summary>
        public int ReplyMessageId { get; set; }

        /// <summary>
        /// Тип исполняющейся команды
        /// </summary>
        public abstract Type CommandType { get; }
    }
}