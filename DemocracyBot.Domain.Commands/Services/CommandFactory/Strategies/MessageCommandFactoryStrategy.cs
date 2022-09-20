using System;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions.Interactive;
using Telegram.Bot.Types;

namespace DemocracyBot.Domain.Commands.Services.CommandFactory.Strategies
{
    public class MessageCommandFactoryStrategy : BaseCommandFactoryStrategy
    {
        public MessageCommandFactoryStrategy(IServiceProvider serviceProvider, IStateManager stateManager)
            : base(serviceProvider, stateManager)
        {
        }

        public override ICommand CreateCommand(Update update)
        {
            var message = update.Message;
            
            if (message == null)
                return null;
            
            var messageText = message.Text?.Replace("@DemocracyDogBot", "");
            
            if (string.IsNullOrWhiteSpace(messageText))
                return null;
            
            if (TryCreateInteractiveCommand(update, out var command))
                return command;
            
            var commandText = messageText[1..].Split(' ')[0];
            
            if (messageText.StartsWith('/') && CommandFactoryHelper.Commands.TryGetValue(commandText, out var commandType))
            {
                var commandBase = CreateCommand(update, commandType);
                command = commandBase;
            }
            
            return command;
        }
        
        private bool TryCreateInteractiveCommand(Update update, out ICommand command)
        {
            var message = update.Message!;
            command = null;

            var state = StateManager.GetState<InteractiveStateBase>(message.From!.Id);
            if (state == null)
                return false;

            var replyToMessageId = message.ReplyToMessage?.MessageId;
            if (replyToMessageId == null || replyToMessageId != state.ReplyMessageId)
            {
                StateManager.RemoveState(message.From.Id);
                return false;
            }

            command = CreateCommand(update, state.CommandType);
            return true;
        }
    }
}