using System.Collections.Concurrent;
using System.Collections.Generic;
using DemocracyBot.Domain.Commands.Abstractions.Interactive;

namespace DemocracyBot.Domain.Commands.Services
{
    public class StateManager : IStateManager
    {
        private readonly IDictionary<long, InteractiveStateBase> _states =
            new ConcurrentDictionary<long, InteractiveStateBase>();


        public TState GetState<TState>(long userId) where TState : InteractiveStateBase
        {
            if (_states.TryGetValue(userId, out var state))
            {
                return (TState) state;
            }

            return null;
        }

        public void RemoveState(long userId)
        {
            _states.Remove(userId);
        }

        public void AddState<TState>(long userId, TState state) where TState : InteractiveStateBase
        {
            _states[userId] = state;
        }
    }
}