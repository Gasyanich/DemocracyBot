using System;
using DemocracyBot.Domain.Commands.Abstractions.Interactive;

namespace DemocracyBot.Domain.Commands.Commands.Meet
{
    public class MeetState : InteractiveStateBase<MeetStep>
    {
        public override Type CommandType => typeof(MeetCommand);

        public string MeetPlace { get; set; }

        public DateTimeOffset MeetDateTime { get; set; }
        
    }
}