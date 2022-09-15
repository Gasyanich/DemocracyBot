using System;
using DemocracyBot.Domain.Commands.Abstractions.Interactive;

namespace DemocracyBot.Domain.Commands.Commands.All
{
    public class AllState : InteractiveStateBase<AllStep>
    {
        public override Type CommandType => typeof(AllCommand);
    }
}