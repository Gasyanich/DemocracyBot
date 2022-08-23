using System.Threading.Tasks;
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
        private readonly TelegramBotClient _telegramBotClient;
        private readonly ILogger<BotController> _logger;


        public BotController(TelegramBotClient telegramBotClient, ILogger<BotController> logger)
        {
            _telegramBotClient = telegramBotClient;
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
            var chatId = update.Message!.Chat.Id;
            var messageText = update.Message.Text!;

            await _telegramBotClient.SendTextMessageAsync(chatId, messageText);

            return Ok();
        }
    }
}