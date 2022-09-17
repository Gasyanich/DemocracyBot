using System;
using System.Threading.Tasks;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Commands;
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


        public async Task Handle(Update message)
        {
            try
            {
                var command = _commandFactory.CreateCommand(message);

                if (command != null)
                    await command.Execute();
            }
            catch (Exception e)
            {
                var handleError = _commandFactory.CreateCommand(message, typeof(HandleErrorCommand));
                await handleError.Execute();
            }
        }
    }
}