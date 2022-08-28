using System.Collections.Generic;
using System.Threading.Tasks;
using DemocracyBot.Integration.Telegram.Dto;

namespace DemocracyBot.Integration.Telegram.Abstractions
{
    public interface IChatService
    {
        Task<List<ChatUserDto>> GetAllChatUsers(long chatId);
    }
}