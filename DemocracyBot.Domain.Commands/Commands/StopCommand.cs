using System.Threading.Tasks;
using DemocracyBot.DataAccess.Repository.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Commands.Common;
using Telegram.Bot;

namespace DemocracyBot.Domain.Commands.Commands
{
    [Command("stop")]
    public class StopCommand : CommandBase
    {
        private readonly IChatRepository _chatRepository;

        public StopCommand(TelegramBotClient client, IChatRepository chatRepository) : base(client)
        {
            _chatRepository = chatRepository;
        }

        public override async Task Execute()
        {
            var chat = await _chatRepository.GetByChatId(ChatId);

            chat.IsNotificationsActivated = false;

            await _chatRepository.UpdateChat(chat);
        }
    }
}