using System.Threading.Tasks;

namespace DemocracyBot.Domain.Commands.Abstractions
{
    public interface ICommand
    {
        Task Execute();
    }
}