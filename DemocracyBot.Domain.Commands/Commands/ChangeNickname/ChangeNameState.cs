using System;
using DemocracyBot.Domain.Commands.Abstractions.Interactive;

namespace DemocracyBot.Domain.Commands.Commands.ChangeNickname
{
    public class ChangeNameState : InteractiveStateBase<ChangeNameStep>
    {
        public override Type CommandType => typeof(ChangeNameCommand);
    }
}