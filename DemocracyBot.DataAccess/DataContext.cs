using DemocracyBot.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemocracyBot.DataAccess
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public virtual DbSet<Chat> Chats { get; set; }

        public virtual DbSet<BotUser> Users { get; set; }
        public virtual DbSet<Meet> Meets { get; set; }
    }
}