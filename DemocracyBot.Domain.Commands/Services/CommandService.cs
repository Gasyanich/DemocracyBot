using System.Threading.Tasks;
using DemocracyBot.Domain.Commands.Abstractions;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DemocracyBot.Domain.Commands.Services
{
    public class CommandService : ICommandService
    {
        private readonly ICommandFactoryService _commandFactoryService;
        private readonly ILogger _logger;

        public CommandService(ICommandFactoryService commandFactoryService, ILogger logger)
        {
            _commandFactoryService = commandFactoryService;
            _logger = logger;
        }


        public async Task Handle(Update update)
        {
            LogUpdateInfo(update);
            
            var command = _commandFactoryService.CreateCommand(update);

            if (command != null)
                await command.Execute();
        }

        private void LogUpdateInfo(Update update)
        {
            var fromUsername = update.Type switch
            {
                UpdateType.Message => update.Message?.From?.Username,
                UpdateType.CallbackQuery => update.CallbackQuery?.From?.Username,
                _ => null
            };

            var messageText = update.Type switch
            {
                UpdateType.Message => update.Message?.Text,
                UpdateType.CallbackQuery => update.CallbackQuery?.Data,
                _ => null
            };


            if (fromUsername != null && messageText != null)
                _logger.LogInformation($"Username: {fromUsername}. Message: {messageText}");
        }
    }
}