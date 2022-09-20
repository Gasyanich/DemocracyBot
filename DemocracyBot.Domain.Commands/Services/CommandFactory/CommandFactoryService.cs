using System;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions.Interactive;
using DemocracyBot.Domain.Commands.Services.CommandFactory.Strategies;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DemocracyBot.Domain.Commands.Services.CommandFactory
{
    public class CommandFactoryService : ICommandFactoryService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IStateManager _stateManager;

        public CommandFactoryService(IServiceProvider serviceProvider, IStateManager stateManager)
        {
            _serviceProvider = serviceProvider;
            _stateManager = stateManager;
        }

        public ICommand CreateCommand(Update update)
        {
            ICommandFactoryStrategy strategy = update.Type switch
            {
                UpdateType.Message => new MessageCommandFactoryStrategy(_serviceProvider, _stateManager),
                UpdateType.CallbackQuery => new CallbackQueryCommandFactoryStrategy(_serviceProvider, _stateManager),
                _ => null
            };

            return strategy?.CreateCommand(update);
        }
    }
}