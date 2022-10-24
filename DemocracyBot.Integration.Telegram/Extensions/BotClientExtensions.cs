using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DemocracyBot.Integration.Telegram.Extensions;

public static class BotClientExtensions
{
    public static async Task ChangeNickName(this TelegramBotClient client,
        long chatId,
        long userId,
        string nickname,
        ChatMemberStatus status)
    {
        if (status != ChatMemberStatus.Administrator)
            await client.PromoteChatMemberAsync(
                chatId,
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

        await client.SetChatAdministratorCustomTitleAsync(chatId, userId, nickname);
    }

    public static async Task MuteUser(this TelegramBotClient client,
        long chatId,
        long userId,
        int durationMin,
        ChatMemberStatus status)
    {
        if (status == ChatMemberStatus.Administrator)
            await client.PromoteChatMemberAsync(chatId,
                userId,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false);


        await client.RestrictChatMemberAsync(
            chatId,
            userId,
            new ChatPermissions
            {
                CanSendMessages = false,
                CanChangeInfo = false,
                CanInviteUsers = false,
                CanPinMessages = false,
                CanSendPolls = false,
                CanSendMediaMessages = false,
                CanSendOtherMessages = false,
                CanAddWebPagePreviews = false
            }, DateTime.Now.AddMinutes(durationMin)
        );
    }
}