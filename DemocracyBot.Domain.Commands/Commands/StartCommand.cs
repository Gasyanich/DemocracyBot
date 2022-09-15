using System.Linq;
using System.Threading.Tasks;
using DemocracyBot.DataAccess.Entities;
using DemocracyBot.DataAccess.Repository.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Commands.Common;
using DemocracyBot.Integration.Telegram.Abstractions;
using Telegram.Bot;

namespace DemocracyBot.Domain.Commands.Commands
{
    [Command("start")]
    public class StartCommand : CommandBase
    {
        private readonly IChatRepository _chatRepository;
        private readonly IChatService _chatService;

        public StartCommand(
            TelegramBotClient client,
            IChatRepository chatRepository,
            IChatService chatService) : base(client)
        {
            _chatRepository = chatRepository;
            _chatService = chatService;
        }

        public override async Task Execute()
        {
            if (await _chatRepository.GetByChatId(ChatId) != null)
                return;

            var chatUsers = await _chatService.GetAllChatUsers(ChatId);

            var botUsers = chatUsers
                .Where(chatUser => chatUser.UserId != Client.BotId)
                .Select(chatUser => new BotUser
                {
                    Id = chatUser.UserId,
                    ChatId = ChatId,
                    Status = UserStatus.Common,
                    Username = chatUser.UserName,
                    ReputationScore = 0,
                    AvailableReputationScore = 15,
                    ReputationVoteErrorCount = 0
                }).ToList();

            await _chatRepository.AddChat(new Chat
            {
                Id = ChatId,
                IsNotificationsActivated = true,
                Users = botUsers
            });

            await SendTextMessage("Добро пожаловать в светлое будущее, дети мои");
        }
    }
}