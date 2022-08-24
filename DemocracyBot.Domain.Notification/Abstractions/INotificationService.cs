using System.Threading.Tasks;
using DemocracyBot.Domain.Notification.Dto;

namespace DemocracyBot.Domain.Notification.Abstractions
{
    public interface INotificationService
    {
        Task SendNotificationToChat(NotificationMessageDto messageDto);
    }
}