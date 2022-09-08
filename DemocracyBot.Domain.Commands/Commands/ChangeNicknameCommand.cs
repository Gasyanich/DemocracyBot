using System;
using System.Threading.Tasks;
using DemocracyBot.Domain.Commands.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace DemocracyBot.Domain.Commands.Commands
{
    [Command("change_name")]
    public class ChangeNicknameCommand : CommandBase
    {
        public ChangeNicknameCommand(TelegramBotClient client) : base(client)
        {
        }

        public override async Task Execute()
        {
            var userId = Message.From!.Id;

            var chatUserInfo = await Client.GetChatMemberAsync(ChatId, userId);

            var newNickName = Message.Text?.Replace("/change_name", "").Trim();

            if (string.IsNullOrEmpty(newNickName))
            {
                await Client.SendTextMessageAsync(ChatId,
                    "Бро, не понял как тебя называть",
                    replyToMessageId: Message.MessageId);

                return;
            }

            if (newNickName.Length > 16)
            {
                await Client.SendTextMessageAsync(ChatId,
                    "Бро, не больше 16 символов, так Пашка завещал",
                    replyToMessageId: Message.MessageId);

                return;
            }

            if (chatUserInfo.Status != ChatMemberStatus.Administrator &&
                chatUserInfo.Status != ChatMemberStatus.Creator)
                await Client.PromoteChatMemberAsync(
                    ChatId,
                    userId,
                    isAnonymous: false,
                    canManageChat: false,
                    canPostMessages: null,
                    canEditMessages: null,
                    canDeleteMessages: false,
                    canManageVideoChats: null,
                    canRestrictMembers: false,
                    canPromoteMembers: false,
                    canChangeInfo: true,
                    canInviteUsers: true,
                    canPinMessages: true);

            if (chatUserInfo.Status == ChatMemberStatus.Creator)
            {
                await Client.SendTextMessageAsync(ChatId, "А ты пошел нахуй, владелец",
                    replyToMessageId: Message.MessageId);

                return;
            }


            try
            {
                await Client.SetChatAdministratorCustomTitleAsync(ChatId, userId, newNickName);
                await Client.SendTextMessageAsync(ChatId, GetRandomReactionOnNewNickname(newNickName),
                    replyToMessageId: Message.MessageId);
            }
            catch (Exception e)
            {
                await Client.SendTextMessageAsync(ChatId,
                    "Бро, я хз че пошло не так, мб ты смайлики напихал, а так нельзя - Пашка завещал",
                    replyToMessageId: Message.MessageId);
            }
        }

        private static string GetRandomReactionOnNewNickname(string newNickName)
        {
            var rnd = new Random();
            var randomNumber = rnd.Next(5);

            return randomNumber switch
            {
                0 => $"Отныне и впредь, звать тебя - {newNickName}",
                1 => "А поновее ничего нет? Но пусть так",
                2 => "Так несмешно же, но я уважаю твой выбор",
                3 => $"Ну, {newNickName}, так {newNickName}",
                4 => $"Ахахаха, класс!"
            };
        }
    }
}