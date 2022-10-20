using System.Threading.Tasks;
using DemocracyBot.DataAccess.Entities;

namespace DemocracyBot.DataAccess.Repository.Abstractions;

public interface IMeetRepository
{
    Task CreateMeet(Meet meet);

    Task UpdateMeet(Meet meet);

    Task<Meet> GetMeetById(long meetId);
}