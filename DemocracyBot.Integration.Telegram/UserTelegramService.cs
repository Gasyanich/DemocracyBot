using System;
using System.Threading;
using System.Threading.Tasks;
using DemocracyBot.Integration.Telegram.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TL;

namespace DemocracyBot.Integration.Telegram
{
    public class UserTelegramService : BackgroundService, IUserTelegramService
    {
        public WTelegram.Client Client { get; set; }
        public User User { get; private set; }
        public Task<string> ConfigNeeded() => _configNeeded.Task;

        private readonly IConfiguration _config;
        private TaskCompletionSource<string> _configNeeded = new TaskCompletionSource<string>();
        private readonly ManualResetEventSlim _configRequest = new ManualResetEventSlim();
        private string _configValue;

        public UserTelegramService(IConfiguration config)
        {
            _config = config;
            Client = new WTelegram.Client(Config);
        }

        private string Config(string what)
        {
            switch (what)
            {
                case "verification_code":
                case "password":
                    _configNeeded.SetResult(what);
                    _configRequest.Wait();
                    _configRequest.Reset();
                    return _configValue;
                default:
                    return _config[what]; // use the ASP.NET configuration (see launchSettings.json)
            }
        }

        public void ReplyConfig(string value)
        {
            _configValue = value;
            _configNeeded = new TaskCompletionSource<string>();
            _configRequest.Set();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                User = await Client.LoginUserIfNeeded();
            }
            catch (Exception ex)
            {
                _configNeeded.SetException(ex);
                throw;
            }

            _configNeeded.SetResult(null); // login complete
        }
    }
}