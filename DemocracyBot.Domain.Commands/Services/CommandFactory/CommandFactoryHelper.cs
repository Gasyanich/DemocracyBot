using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions.CommandsBase;

namespace DemocracyBot.Domain.Commands.Services.CommandFactory
{
    public static class CommandFactoryHelper
    {
        
        
        public static Dictionary<string, Type> Commands =>
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
    }
}