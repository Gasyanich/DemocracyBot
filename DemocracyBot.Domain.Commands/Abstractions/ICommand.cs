using System.Threading.Tasks;
using DemocracyBot.Domain.Commands.Commands.Common;

namespace DemocracyBot.Domain.Commands.Abstractions
{
    public interface ICommand
    {
        Task Execute();

        CommandType Type { get; }
    }
}