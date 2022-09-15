using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace DemocracyBot.Domain.Commands.Abstractions.Interactive
{
    public abstract class InteractiveCommandBase<TState, TStep> : CommandBase
        where TState : InteractiveStateBase<TStep>, new()
        where TStep : Enum
    {
        private readonly IStateManager _stateManager;

        protected InteractiveCommandBase(TelegramBotClient client, IStateManager stateManager) : base(client)
        {
            _stateManager = stateManager;
        }

        protected TState State => _stateManager.GetState<TState>(UserId);

        /// <summary>
        /// Обработка первого шага
        /// </summary>
        protected abstract Task<TStep> HandleStart();

        /// <summary>
        /// Обработка опредленного шага
        /// </summary>
        /// <param name="step">Текущий шаг</param>
        /// <returns>Следующий шаг</returns>
        protected abstract Task<TStep> HandleStep(TStep step);


        public override async Task Execute()
        {
            if (State == null)
            {
                _stateManager.AddState(UserId, new TState());

                State!.CurrentStep = await HandleStart();

                return;
            }

            State.CurrentStep = await HandleStep(State.CurrentStep);

            if (State.IsLastStep)
                _stateManager.RemoveState(UserId);
        }


        protected override async Task<Message> Reply(string messageText, IReplyMarkup replyMarkup = null)
        {
            var message = await base.Reply(messageText, replyMarkup);

            State.ReplyMessageId = message.MessageId;

            return message;
        }
    }
}