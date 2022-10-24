using System.Threading.Tasks;
using DemocracyBot.DataAccess.Entities;

namespace DemocracyBot.Domain.Commands.Abstractions;

public interface IRestrictService
{
    Task RestrictUserByPoll(int pollMessageId, long chatId, BotUser user);
}