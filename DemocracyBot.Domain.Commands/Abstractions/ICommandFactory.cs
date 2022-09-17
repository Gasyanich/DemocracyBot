using System;
using Telegram.Bot.Types;

namespace DemocracyBot.Domain.Commands.Abstractions
{
    public interface ICommandFactory
    {
        ICommand CreateCommand(Update update);

        ICommand CreateCommand(Update update, Type commandType);
    }
}