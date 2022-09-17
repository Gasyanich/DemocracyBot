using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions.Interactive;
using DemocracyBot.Domain.Commands.Commands;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;

namespace DemocracyBot.Domain.Commands
{
    public class CommandFactory : ICommandFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IStateManager _stateManager;

        public CommandFactory(IServiceProvider serviceProvider, IStateManager stateManager)
        {
            _serviceProvider = serviceProvider;
            _stateManager = stateManager;
        }

        public ICommand CreateCommand(Message message)
        {
            if (message == null)
                return null;
            
            var messageText = message.Text;

            if (string.IsNullOrWhiteSpace(messageText))
                return null;
            
            if (TryCreateInteractiveCommand(message, out var command))
                return command;
            
            var commandText = messageText[1..].Split(' ')[0];
            
            if (messageText.StartsWith('/') && CommandTextToCommandType.TryGetValue(commandText, out var commandType))
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

        private bool TryCreateInteractiveCommand(Message message, out ICommand command)
        {
            command = null;

            var state = _stateManager.GetState<InteractiveStateBase>(message.From!.Id);
            if (state == null)
                return false;

            var replyToMessageId = message.ReplyToMessage?.MessageId;
            if (replyToMessageId == null || replyToMessageId != state.ReplyMessageId)
            {
                _stateManager.RemoveState(message.From.Id);
                return false;
            }
            
            command = CreateCommand(message, state.CommandType);
            return true;

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