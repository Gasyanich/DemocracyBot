using System;
using System.Threading.Tasks;
using DemocracyBot.DataAccess.Repository.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DemocracyBot.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BotController : ControllerBase
    {
        private readonly IChatRepository _chatRepository;
        private readonly ICommandService _commandService;
        private readonly ILogger<BotController> _logger;
        private readonly TelegramBotClient _botClient;

        public BotController(IChatRepository chatRepository,
            ICommandService commandService,
            ILogger<BotController> logger, 
            TelegramBotClient botClient)
        {
            _chatRepository = chatRepository;
            _commandService = commandService;
            _logger = logger;
            _botClient = botClient;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Started");
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] Update update)
        {
            _logger.LogInformation("Get telegram update");
            await _botClient.SendTextMessageAsync(update.Message!.Chat.Id, update!.Message.Text ?? "pusto");
            
            try
            {
                await _commandService.Handle(update.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
            
                return Ok();
            }

            return Ok();
        }

        [HttpGet("chats")]
        public async Task<IActionResult> GetChats()
        {
            return Ok(await _chatRepository.GetChats());
        }
    }
}