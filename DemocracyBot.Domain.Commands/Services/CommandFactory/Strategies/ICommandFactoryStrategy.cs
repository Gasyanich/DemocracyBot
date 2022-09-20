using DemocracyBot.Domain.Commands.Abstractions;
using Telegram.Bot.Types;

namespace DemocracyBot.Domain.Commands.Services.CommandFactory.Strategies
{
    public interface ICommandFactoryStrategy
    {
        ICommand CreateCommand(Update update);
    }
}