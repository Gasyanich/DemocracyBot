using System.Threading.Tasks;
using DemocracyBot.DataAccess.Entities;
using DemocracyBot.DataAccess.Repository.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions;
using Telegram.Bot;

namespace DemocracyBot.Domain.Commands.Commands
{
    public class StartCommand : CommandBase
    {
        private readonly IChatRepository _chatRepository;

        public StartCommand(TelegramBotClient client, IChatRepository chatRepository) : base(client)
        {
            _chatRepository = chatRepository;
        }

        public override async Task Execute()
        {
            if (await _chatRepository.GetByChatId(ChatId) != null)
                return;

            await _chatRepository.AddChat(new Chat
            {
                Id = ChatId,
                IsNotificationsActivated = true
            });

            await SendTextMessage("Добро пожаловать в светлое будущее, дети мои");
        }
    }
}