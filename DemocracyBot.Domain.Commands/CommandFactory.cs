using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            var commandText = messageText[1..].Split(' ')[0];

            if (CommandTextToCommandType.TryGetValue(commandText, out var commandType))
            {
                var commandBase = CreateCommand(message, commandType);
                command = commandBase;
            }

            return command;
        }

        private ICommand CreateCommand(Message message, Type commandType) 
        {
            var scope = _serviceProvider.CreateScope();

            var commandBase = (CommandBase) scope.ServiceProvider.GetRequiredService(commandType);
            commandBase.Init(message);

            return commandBase;
        }

        #region Init CommandDictionary

        private static Dictionary<string, Type> CommandTextToCommandType =>
            _commandTextToCommandType ??= InitCommandTextToCommandType();

        private static Dictionary<string, Type> _commandTextToCommandType;

        private static Dictionary<string, Type> InitCommandTextToCommandType()
        {
            var commandTextToCommandType = new Dictionary<string, Type>();

            var commandTypes = typeof(CommandBase).Assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface && t.IsSubclassOf(typeof(CommandBase)));

            foreach (var commandType in commandTypes)
            {
                var attr = commandType.GetCustomAttribute<CommandAttribute>();
                if (attr == null)
                    throw new Exception("Command without attribute " + commandType.Name);

                commandTextToCommandType.Add(attr.CommandText, commandType);
            }

            return commandTextToCommandType;
        }

        #endregion
    }
}