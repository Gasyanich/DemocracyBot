using System.Collections.Generic;
using System.Threading.Tasks;
using DemocracyBot.DataAccess.Entities;

namespace DemocracyBot.DataAccess.Repository.Abstractions
{
    public interface IChatRepository
    {
        Task<Chat> AddChat(Chat chat);

        Task<Chat> UpdateChat(Chat chat);

        Task<Chat> GetByChatId(long chatId);

        Task<IEnumerable<Chat>> GetChats();
    }
}