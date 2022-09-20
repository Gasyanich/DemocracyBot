using System;
using Telegram.Bot.Types;

namespace DemocracyBot.Domain.Commands.Abstractions
{
    public interface ICommandFactoryService
    {
        ICommand CreateCommand(Update update);
        
    }
}