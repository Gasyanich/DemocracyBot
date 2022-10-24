using System;
using DemocracyBot.Domain.Commands.Abstractions.Interactive;

namespace DemocracyBot.Domain.Commands.Commands.Restrict;

public class RestrictState : InteractiveStateBase<RestrictStep>
{
    public RestrictType RestrictType { get; set; }

    public override Type CommandType => RestrictType switch
    {
        RestrictType.Mute => typeof(MuteCommand),
        RestrictType.Ban => typeof(BanCommand),
        _ => throw new ArgumentOutOfRangeException()
    };
}