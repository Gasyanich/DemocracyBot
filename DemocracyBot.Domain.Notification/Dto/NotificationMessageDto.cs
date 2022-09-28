using System.Collections.Generic;

namespace DemocracyBot.Domain.Notification.Dto
{
    public class NotificationMessageDto
    {
        public long ChatId { get; set; }

        public string MessageText { get; set; }

        public string AfterMessageStickerFileId { get; set; }

        public string BeforeMessageStickerFileId { get; set; }

        public List<string> StickerFIleIds { get; set; } = new List<string>();

        public bool IsPinMessage { get; set; }
    }
}