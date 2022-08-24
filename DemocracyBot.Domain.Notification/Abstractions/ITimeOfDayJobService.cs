using System.Threading.Tasks;

namespace DemocracyBot.Domain.Notification.Abstractions
{
    public interface ITimeOfDayJobService
    {
        Task GoodMorningJob();

        Task GoodNightJob();
    }
}