using System.Linq;
using DemocracyBot.Domain.Commands.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DemocracyBot.Domain.Commands
{
    public static class Entry
    {
        public static IServiceCollection AddCommands(this IServiceCollection services)
        {
            services.AddScoped<ICommandFactory, CommandFactory>();
            services.AddScoped<ICommandService, CommandService>();

            var commandTypes = typeof(CommandBase).Assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface && t.IsSubclassOf(typeof(CommandBase)));

            foreach (var commandType in commandTypes)
                services.AddScoped(commandType);


            return services;
        }
    }
}