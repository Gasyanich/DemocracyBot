using System;
using System.Linq;
using System.Threading.Tasks;
using DemocracyBot.Integration.Telegram;
using Microsoft.AspNetCore.Mvc;
using TL;

namespace DemocracyBot.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserBotController : ControllerBase
    {
        private readonly UserTelegramService _wt;

        public UserBotController(UserTelegramService wt) => _wt = wt;

        [HttpGet("status")]
        public async Task<ContentResult> Status()
        {
            var config = await _wt.ConfigNeeded();
            if (config != null)
                return Content($@"Enter {config}: <form action=""config""><input name=""value"" autofocus/></form>",
                    "text/html");
            else
                return Content($@"Connected as {_wt.User}");
        }

        [HttpGet("config")]
        public ActionResult Config(string value)
        {
            _wt.ReplyConfig(value);
            return Redirect("status");
        }
    }
}