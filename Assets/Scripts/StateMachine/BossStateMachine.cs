using System;
using System.Collections.Generic;
using StateMachine.States;

namespace StateMachine
{
    public class BossStateMachine : GameStateMachine
    {
        protected override void Awake()
        {
            _states = new Dictionary<Type, IState>();
            foreach (BaseStates state in _gameStates)
                _states.Add(state.GetType(), state);
        }
    }
}