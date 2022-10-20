using System.Threading.Tasks;
using DemocracyBot.DataAccess.Entities;
using DemocracyBot.DataAccess.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DemocracyBot.DataAccess.Repository;

public class MeetRepository : IMeetRepository
{
    private readonly DataContext _dataContext;

    public MeetRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task CreateMeet(Meet meet)
    {
        _dataContext.Meets.Add(meet);

        await _dataContext.SaveChangesAsync();
    }

    public async Task UpdateMeet(Meet meet)
    {
        _dataContext.Meets.Update(meet);
        _dataContext.AttachRange(meet.Users);

        await _dataContext.SaveChangesAsync();
    }

    public async Task<Meet> GetMeetById(long meetId)
    {
        return await _dataContext.Meets
            .Include(meet => meet.Users)
            .FirstOrDefaultAsync(m => m.Id == meetId);
    }
}