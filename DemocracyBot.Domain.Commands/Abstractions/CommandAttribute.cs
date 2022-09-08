using System;

namespace DemocracyBot.Domain.Commands.Abstractions
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