using System;
using System.Threading.Tasks;
using DemocracyBot.DataAccess.Entities;
using DemocracyBot.DataAccess.Repository.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public BotController(IChatRepository chatRepository,
            ICommandService commandService,
            ILogger<BotController> logger)
        {
            _chatRepository = chatRepository;
            _commandService = commandService;
            _logger = logger;
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

            try
            {
                await _commandService.Handle(update);
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

        [HttpGet("delete")]
        public async Task<IActionResult> DeleteChats()
        {
            await _chatRepository.DeleteChats();

            return Ok();
        }
    }
}