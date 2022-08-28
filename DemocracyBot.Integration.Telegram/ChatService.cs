using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DemocracyBot.Integration.Telegram.Abstractions;
using DemocracyBot.Integration.Telegram.Dto;
using Microsoft.Extensions.Logging;
using TL;
using WTelegram;

namespace DemocracyBot.Integration.Telegram
{
    public class ChatService : IChatService
    {
        private readonly IUserTelegramService _userTelegramService;
        private Client Client => _userTelegramService.Client;

        private readonly ILogger<ChatService> _logger;


        public ChatService(IUserTelegramService userTelegramService, ILogger<ChatService> logger)
        {
            _userTelegramService = userTelegramService;
            _logger = logger;
        }

        public async Task<List<ChatUserDto>> GetAllChatUsers(long chatId)
        {
            _logger.LogInformation("BotChatId " + chatId);
            var chatUsers = new List<ChatUserDto>();

            var chats = await Client.Messages_GetAllChats();
            var userChatId = long.Parse(chatId.ToString()[4..]);

            _logger.LogInformation("UserChatId " + userChatId);


            var channel = (Channel) chats.chats[userChatId];

            for (int offset = 0;;)
            {
                var participants = await Client.Channels_GetParticipants(channel, null, offset);

                foreach (var (id, user) in participants.users)
                    chatUsers.Add(new ChatUserDto(id, user.username));


                offset += participants.participants.Length;
                if (offset >= participants.count) break;
            }

            return chatUsers;
        }
    }
}