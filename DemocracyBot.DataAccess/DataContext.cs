﻿using DemocracyBot.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemocracyBot.DataAccess
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Chat> Chats { get; set; }
    }
}