using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemocracyBot.DataAccess.Repository.Abstractions;
using DemocracyBot.Domain.Notification.Abstractions;
using DemocracyBot.Domain.Notification.Dto;

namespace DemocracyBot.Domain.Notification
{
    public class BirthDayNotificationService : IBirthDayNotificationService
    {
        private readonly IChatRepository _chatRepository;
        private readonly INotificationService _notificationService;

        public BirthDayNotificationService(IChatRepository chatRepository, INotificationService notificationService)
        {
            _chatRepository = chatRepository;
            _notificationService = notificationService;
        }

        public async Task HappyBirthDay()
        {
            var chats = await _chatRepository.GetActiveChats();

            foreach (var chat in chats)
            {
                var today = DateTime.Today.AddDays(1);

                var users = chat.Users.Where(u => u.BirthDate.Month == today.Month && u.BirthDate.Day == today.Day);

                foreach (var user in users)
                {
                    var age = DateTime.Now.Year - user.BirthDate.Year;

                    await _notificationService.SendNotificationToChat(new NotificationMessageDto
                    {
                        ChatId = chat.Id,
                        IsPinMessage = true,
                        MessageText = GetMessage(age, user.Id),
                        StickerFIleIds = new List<string>
                        {
                            "CAACAgIAAxkBAAEF4uRjKe0WmbTrO0918G-Co3eNsdws_gACahgAAm0x0UnEbL97b6acmSkE",
                            "CAACAgIAAxkBAAEF4uZjKe1fQ35AyD9YDTO-FIm5Jf7foQAChxoAAr1NOEq9sPjp-eU-CSkE",
                            "CAACAgIAAxkBAAEF4uhjKe2E5NhTgvCY7MWJZFnpFarR1gACIxoAAszTeUp2msgwpbUbqykE"
                        }
                    });
                }
            }
        }


        private static string GetMessage(int age, long userId)
        {
            return $"<a href=\"tg://user?id={userId}\">Братишка</a> " +
                   $"тебе сегодня {age}, вот это нихуя себе!" +
                   $"\nОт лица ИИ этого чатика поздравляю тебя с этим замечательным днем";
        }
    }
}