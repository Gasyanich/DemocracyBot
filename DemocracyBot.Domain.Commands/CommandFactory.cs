using System;
using System.Collections.Generic;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Commands;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;

namespace DemocracyBot.Domain.Commands
{
    public class CommandFactory : ICommandFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICommand CreateCommand(Message message)
        {
            var messageText = message.Text;

            ICommand command = null;
            
            if (messageText == null || !messageText.StartsWith('/'))
                return command;

            var commandText = messageText[1..];

            var commandType = CommandTextToCommandType[commandText];

            if (commandType == null)
                return command;

            var scope = _serviceProvider.CreateScope();

            var commandBase = (CommandBase) scope.ServiceProvider.GetRequiredService(commandType);
            commandBase.Init(message);

            command = commandBase;

            return command;
        }

        private static readonly Dictionary<string, Type> CommandTextToCommandType = new Dictionary<string, Type>
        {
            {"start", typeof(StartCommand)}
        };
    }
}