using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemocracyBot.DataAccess.Entities;
using DemocracyBot.DataAccess.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DemocracyBot.DataAccess.Repository
{
    public class ChatRepository : IChatRepository
    {
        private readonly DataContext _context;

        public ChatRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Chat> GetByChatId(long chatId)
        {
            return await _context.Chats
                .Include(chat => chat.Users)
                .FirstOrDefaultAsync(chat => chat.Id == chatId);
        }


        public async Task<IEnumerable<Chat>> GetActiveChats()
        {
            return await _context.Chats
                .Include(chat => chat.Users)
                .Where(c => c.IsNotificationsActivated).ToListAsync();
        }

        public async Task<BotUser> GetChatUserByUserName(long chatId, string userName)
        {
            return await _context.Chats
                .Where(chat => chat.Id == chatId)
                .SelectMany(chat => chat.Users)
                .FirstOrDefaultAsync(user => user.Username == userName);
        }
    }
}