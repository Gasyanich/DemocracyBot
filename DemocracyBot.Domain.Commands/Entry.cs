using System.Linq;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions.CommandsBase;
using DemocracyBot.Domain.Commands.Abstractions.Interactive;
using DemocracyBot.Domain.Commands.Services;
using DemocracyBot.Domain.Commands.Services.CommandFactory;
using Microsoft.Extensions.DependencyInjection;

namespace DemocracyBot.Domain.Commands
{
    public static class Entry
    {
        public static IServiceCollection AddCommands(this IServiceCollection services)
        {
            services.AddScoped<ICommandFactoryService, CommandFactoryService>();
            services.AddScoped<ICommandService, CommandService>();
            services.AddSingleton<IStateManager, StateManager>();

            var commandTypes = typeof(CommandBase).Assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface && t.IsSubclassOf(typeof(CommandBase)));

            foreach (var commandType in commandTypes)
                services.AddScoped(commandType);


            return services;
        }
    }
}