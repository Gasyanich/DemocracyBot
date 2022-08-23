using System.Threading.Tasks;
using DemocracyBot.Domain.Commands.Abstractions;
using Telegram.Bot.Types;

namespace DemocracyBot.Domain.Commands
{
    public class CommandService : ICommandService
    {
        private readonly ICommandFactory _commandFactory;

        public CommandService(ICommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
        }


        public async Task Handle(Message message)
        {
            var command = _commandFactory.CreateCommand(message);

            if (command != null)
                await command.Execute();
        }
    }
}