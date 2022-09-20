using System.Threading.Tasks;
using DemocracyBot.Domain.Commands.Abstractions;
using Telegram.Bot.Types;

namespace DemocracyBot.Domain.Commands.Services
{
    public class CommandService : ICommandService
    {
        private readonly ICommandFactoryService _commandFactoryService;

        public CommandService(ICommandFactoryService commandFactoryService)
        {
            _commandFactoryService = commandFactoryService;
        }


        public async Task Handle(Update message)
        {
            var command = _commandFactoryService.CreateCommand(message);

            if (command != null)
                await command.Execute();
        }
    }
}