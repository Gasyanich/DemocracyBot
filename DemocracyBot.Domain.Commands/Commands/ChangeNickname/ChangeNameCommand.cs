using System;
using System.Threading.Tasks;
using DemocracyBot.Domain.Commands.Abstractions.Interactive;
using DemocracyBot.Domain.Commands.Commands.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace DemocracyBot.Domain.Commands.Commands.ChangeNickname
{
    [Command("change_name")]
    public class ChangeNameCommand : InteractiveCommandBase<ChangeNameState, ChangeNameStep>
    {
        public ChangeNameCommand(TelegramBotClient client, IStateManager stateManager) : base(client, stateManager)
        {
        }

        protected override async Task<ChangeNameStep> HandleStart()
        {
            await Reply("Как нам тебя теперь называть?", AskNewNicknameMarkup);

            return ChangeNameStep.SelectNickname;
        }

        protected override async Task<ChangeNameStep> HandleStep(ChangeNameStep step)
        {
            if (step == ChangeNameStep.SelectNickname)
            {
                const ChangeNameStep onErrorStep = ChangeNameStep.SelectNickname;

                var newNickName = Message.Text?.Trim();

                var chatMember = await Client.GetChatMemberAsync(ChatId, UserId);
                if (chatMember.Status == ChatMemberStatus.Creator)
                {
                    await Reply("А ты пошел нахуй, владелец");
                    return default;
                }
                
                if (!await ValidateNickname(newNickName))
                    return onErrorStep;

                try
                {
                    await SetAdmin(chatMember);
                    await Client.SetChatAdministratorCustomTitleAsync(ChatId, UserId, newNickName!);

                    await Client.SendTextMessageAsync(
                        ChatId,
                        GetRandomReactionOnNewNickname(newNickName),
                        replyToMessageId: Message.MessageId
                    );
                }
                catch (Exception e)
                {
                    await Reply("Бро, я хз че пошло не так, мб ты смайлики напихал, а так нельзя - Пашка завещал\n" +
                                "Го ещё раз", AskNewNicknameMarkup);

                    return onErrorStep;
                }
            }

            return default;
        }

        private async Task<bool> ValidateNickname(string nickName)
        {
            if (string.IsNullOrEmpty(nickName))
            {
                await Reply("Бро, не понял как тебя называть", AskNewNicknameMarkup);
                return false;
            }

            if (nickName.Length > 16)
            {
                await Reply("Бро, не больше 16 символов, так Пашка завещал", AskNewNicknameMarkup);
                return false;
            }

            return true;
        }

        private async Task SetAdmin(ChatMember chatMember)
        {
            if (chatMember.Status != ChatMemberStatus.Administrator)
                await Client.PromoteChatMemberAsync(
                    ChatId,
                    UserId,
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

        private static IReplyMarkup AskNewNicknameMarkup => new ForceReplyMarkup
        {
            Selective = true,
            InputFieldPlaceholder = "Введи новое имя здесь"
        };
    }
}