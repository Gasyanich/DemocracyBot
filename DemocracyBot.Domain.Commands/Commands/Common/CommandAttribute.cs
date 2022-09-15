using System;

namespace DemocracyBot.Domain.Commands.Commands.Common
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public CommandAttribute(string commandText)
        {
            CommandText = commandText;
        }

        public string CommandText { get; }
    }
}