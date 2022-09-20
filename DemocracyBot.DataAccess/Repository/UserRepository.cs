using System.Threading.Tasks;
using DemocracyBot.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemocracyBot.DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<BotUser> GetUser(long chatId, long userId)
            => await _dataContext.Users.FirstOrDefaultAsync(user => user.Id == userId && user.ChatId == chatId);


        public async Task<BotUser> CreateUser(BotUser user)
        {
            _dataContext.Users.Add(user);

            await _dataContext.SaveChangesAsync();

            return user;
        }

        public async Task<BotUser> UpdateUser(BotUser user)
        {
            _dataContext.Users.Update(user);

            await _dataContext.SaveChangesAsync();

            return user;
        }
    }
}