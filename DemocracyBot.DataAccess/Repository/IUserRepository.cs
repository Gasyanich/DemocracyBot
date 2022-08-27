using System.Threading.Tasks;
using DemocracyBot.DataAccess.Entities;

namespace DemocracyBot.DataAccess.Repository
{
    public interface IUserRepository
    {
        Task<BotUser> GetUser(long chatId, string userName);

        Task<BotUser> CreateUser(BotUser user);

        Task<BotUser> UpdateUser(BotUser user);
    }
}