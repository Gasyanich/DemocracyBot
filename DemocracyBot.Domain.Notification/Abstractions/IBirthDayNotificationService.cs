using System.Threading.Tasks;

namespace DemocracyBot.Domain.Notification.Abstractions
{
    public interface IBirthDayNotificationService
    {
        Task HappyBirthDay();
    }
}