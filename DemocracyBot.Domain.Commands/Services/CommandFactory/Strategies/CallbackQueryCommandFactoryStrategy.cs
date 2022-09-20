using System;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions.Interactive;
using Telegram.Bot.Types;

namespace DemocracyBot.Domain.Commands.Services.CommandFactory.Strategies
{
    public class CallbackQueryCommandFactoryStrategy : BaseCommandFactoryStrategy
    {
        public CallbackQueryCommandFactoryStrategy(IServiceProvider serviceProvider, IStateManager stateManager)
            : base(serviceProvider, stateManager)
        {
        }

        public override ICommand CreateCommand(Update update)
        {
            var commandText = update.CallbackQuery!.Data;
            if (commandText == null) return null;

            return CommandFactoryHelper.Commands.TryGetValue(commandText[1..], out var commandType)
                ? CreateCommand(update, commandType)
                : null;
        }
    }
}