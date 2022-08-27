using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace DemocracyBot.LongPolling
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // using var client = new WTelegram.Client(Config);
            // var my = await client.LoginUserIfNeeded();
            //
            // Console.WriteLine($"We are logged-in as {my.username ?? my.first_name + " " + my.last_name} (id {my.id})");
            
            var botClient = new TelegramBotClient("5735722044:AAG9S5xRoA26_Jj1DLUYaeF2E6CL6tmfffg");
            
            using var cts = new CancellationTokenSource();
            
            await botClient.DeleteWebhookAsync();
            
            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
            };
            botClient.StartReceiving(
                updateHandler: async (client, update, ct) =>
                {
                    var chatId = update.Message!.Chat.Id;
                    var message = update.Message;

                    var replyText = 
                        $"[Чмо без логина](tg://user?id={886554524}) Эу сука";
                    
                    await botClient.SendTextMessageAsync(chatId, replyText, ParseMode.MarkdownV2);
                },
                pollingErrorHandler: (client, exception, arg3) =>
                {
                    var a = 5;
                },
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );
            
            var me = await botClient.GetMeAsync();
            
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            
            // Send cancellation request to stop bot
            cts.Cancel();
        }
        
        static string Config(string what)
        {
            switch (what)
            {
                case "api_id": return "2227391";
                case "api_hash": return "81f7429140163b499a496abdcc49db2e";
                case "phone_number": return "+79023217238";
                case "verification_code": Console.Write("Code: "); return Console.ReadLine();
                case "first_name": return "Игорь";      // if sign-up is required
                case "last_name": return "Игуменов";        // if sign-up is required
                case "password": return "secret!";     // if user has enabled 2FA
                default: return null;                  // let WTelegramClient decide the default config
            }
        }
    }
}