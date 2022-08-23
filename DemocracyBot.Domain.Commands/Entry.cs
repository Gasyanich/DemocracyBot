using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DemocracyBot.Domain.Commands
{
    public static class Entry
    {
        public static IServiceCollection AddCommands(this IServiceCollection services)
        {
            services.AddScoped<ICommandFactory, CommandFactory>();
            services.AddScoped<ICommandService, CommandService>();

            services.AddScoped<StartCommand>();
            
            return services;
        }
    }
}