using System;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions.CommandsBase;
using DemocracyBot.Domain.Commands.Abstractions.Interactive;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;

namespace DemocracyBot.Domain.Commands.Services.CommandFactory.Strategies
{
    public abstract class BaseCommandFactoryStrategy : ICommandFactoryStrategy
    {
        private readonly IServiceProvider _serviceProvider;
        protected readonly IStateManager StateManager;

        protected BaseCommandFactoryStrategy(IServiceProvider serviceProvider, IStateManager stateManager)
        {
            _serviceProvider = serviceProvider;
            StateManager = stateManager;
        }

        public abstract ICommand CreateCommand(Update update);

        protected ICommand CreateCommand(Update update, Type commandType)
        {
            var scope = _serviceProvider.CreateScope();

            var commandBase = (CommandBase) scope.ServiceProvider.GetRequiredService(commandType);
            commandBase.Init(update);

            return commandBase;
        }
    }
}