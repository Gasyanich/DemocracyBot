namespace DemocracyBot.Domain.Commands.Abstractions.Interactive
{
    public interface IStateManager
    {
        TState GetState<TState>(long userId) where TState : InteractiveStateBase;

        void RemoveState(long userId);

        void AddState<TState>(long userId, TState state) where TState : InteractiveStateBase;
    }
}