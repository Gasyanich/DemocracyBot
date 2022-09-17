using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace DemocracyBot.Domain.Commands.Abstractions
{
    public interface ICommandService
    {
        Task Handle(Update message);
    }
}