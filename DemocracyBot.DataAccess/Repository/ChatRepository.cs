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


        public async Task<Chat> AddChat(Chat chat)
        {
            _context.Chats.Add(chat);

            await _context.SaveChangesAsync();

            return chat;
        }

        public async Task<Chat> UpdateChat(Chat chat)
        {
            _context.Chats.Update(chat);

            await _context.SaveChangesAsync();

            return chat;
        }

        public async Task<Chat> GetByChatId(long chatId)
        {
            return await _context.Chats
                .Include(chat => chat.Users)
                .FirstOrDefaultAsync(chat => chat.Id == chatId);
        }

        public async Task<IEnumerable<Chat>> GetChats()
        {
            return await _context.Chats.Include(chat => chat.Users).ToListAsync();
        }

        public async Task<IEnumerable<Chat>> GetActiveChats()
        {
            return await _context.Chats
                .Include(chat => chat.Users)
                .Where(c => c.IsNotificationsActivated).ToListAsync();
        }

        public async Task DeleteChats()
        {
            var chats = await _context.Chats
                .Include(chat => chat.Users)
                .ToListAsync();
            
            _context.Chats.RemoveRange(chats);

            await _context.SaveChangesAsync();
        }
    }
}