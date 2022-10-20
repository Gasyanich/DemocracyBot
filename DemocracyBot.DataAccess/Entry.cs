using System;
using DemocracyBot.DataAccess.Repository;
using DemocracyBot.DataAccess.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DemocracyBot.DataAccess
{
    public static class Entry
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(builder =>
                builder.UseSqlServer(configuration.GetConnectionString("Db")));

            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IMeetRepository, MeetRepository>();

            using var serviceProvider = services.BuildServiceProvider();
            using var dataContext = serviceProvider.GetRequiredService<DataContext>();

            dataContext.Database.Migrate();

            return services;
        }
    }
}