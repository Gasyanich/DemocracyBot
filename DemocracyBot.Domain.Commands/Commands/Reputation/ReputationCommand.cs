using System;
using System.Threading.Tasks;
using DemocracyBot.DataAccess.Entities;
using DemocracyBot.DataAccess.Repository;
using DemocracyBot.Domain.Commands.Abstractions;
using Telegram.Bot;

namespace DemocracyBot.Domain.Commands.Commands.Reputation
{
    public abstract class ReputationCommand : CommandBase
    {
        private readonly IUserRepository _userRepository;

        protected ReputationCommand(TelegramBotClient client, IUserRepository userRepository) : base(client)
        {
            _userRepository = userRepository;
        }

        protected abstract ReputationDirection ReputationDirection { get; }

        public override async Task Execute()
        {
            var commandParams = Message.Text?.Split(' ');
            if (commandParams == null)
                return;

            var userNameRepTo = commandParams[1][1..];
            var userNameRepFrom = Message.From!.Username;

            var userRepTo = await _userRepository.GetUser(ChatId, userNameRepTo);
            var userRepFrom = await _userRepository.GetUser(ChatId, userNameRepFrom);

            if (userRepFrom.AvailableReputationScore < 0)
            {
                await HandleUserHasNoAvailableScore(userRepFrom);
                return;
            }
        }

        private async Task ChangeUserScore(BotUser user, int score)
        {
            var scoreToChange = ReputationDirection switch
            {
                ReputationDirection.Plus => score,
                ReputationDirection.Minus => -score,
            };

            user.ReputationScore += scoreToChange;
            await _userRepository.UpdateUser(user);
            
            var textMessage = ReputationDirection switch
            {
                ReputationDirection.Plus => $"Репутация @{user.Username} повышена на {score} поинтов и составляет {user.ReputationScore}",
                ReputationDirection.Minus => $"Репутация @{user.Username} понижена на {score} поинтов и составляет {user.ReputationScore} (но ИИ не осуждает)",
            };

            await Client.SendTextMessageAsync(ChatId, textMessage);
        }

        private async Task HandleUserHasNoAvailableScore(BotUser repFrom)
        {
            var reputationVoteErrorCount = repFrom.ReputationVoteErrorCount;
            var textMessage = reputationVoteErrorCount  switch
            {
                0 => "Гражданин, у Вас кончилось право голоса, приходите через недельку",
                1 => "Говорю же - через недельку",
                2 => "Предупреждаю, уебу",
                3 => "Уебал, проверяй",
                // todo: можно сделать > 4 в C# 9 и бесконечно инкрементить, а не как сейчас ограничить 4
                4 => "Ты реально тупой? Мне не жалко, лови",
                // todo: мутить можно толкьо по ид, которого у бота нет 
                //5 => "Чел, ты в муте"
            };

            if (reputationVoteErrorCount != 4)
                repFrom.ReputationVoteErrorCount += 1;

            if (reputationVoteErrorCount == 2)
                await ChangeUserScore(repFrom, 3);
            
            if (reputationVoteErrorCount == 3)
                await ChangeUserScore(repFrom, 5);

            
            
            
            await Client.SendTextMessageAsync(
                ChatId,
                textMessage,
                replyToMessageId: Message.MessageId
            );
        }
    }
}