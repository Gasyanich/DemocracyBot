using System.Collections.Generic;
using System.Threading.Tasks;
using DemocracyBot.DataAccess.Entities;

namespace DemocracyBot.DataAccess.Repository.Abstractions
{
    public interface IChatRepository
    {
        Task<Chat> GetByChatId(long chatId);

        Task<IEnumerable<Chat>> GetActiveChats();
    }
}