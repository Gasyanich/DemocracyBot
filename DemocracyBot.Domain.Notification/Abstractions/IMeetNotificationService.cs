using System.Threading.Tasks;

namespace DemocracyBot.Domain.Notification.Abstractions;

public interface IMeetNotificationService
{
    Task NotifyBeforeMeet(long meetId);
    Task NotifyMeetStart(long meetId);

}