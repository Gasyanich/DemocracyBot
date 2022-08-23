using Telegram.Bot.Types;

namespace DemocracyBot.Domain.Commands.Abstractions
{
    public interface ICommandFactory
    {
        ICommand CreateCommand(Message message);
    }
}