using System;
using System.Collections.Generic;
using StateMachine.States;
using UnityEngine;

namespace StateMachine
{
    public class GameStateMachine : MonoBehaviour
    {
        [Header("Стейты")]
        [SerializeField] protected List<BaseStates> _gameStates;

        protected Dictionary<Type, IState> _states;
        
        public IState ActiveState { get; private set; }

        protected virtual void Awake()
        {
            _states = new Dictionary<Type, IState>();
            foreach (BaseStates state in _gameStates)
                _states.Add(state.GetType(), state);

            Enter<InitialState>();
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        private TState ChangeState<TState>() where TState : class, IState
        {
            ActiveState?.Exit();

            TState state = GetState<TState>();
            ActiveState = state;
            return state;
        }

        public bool CheckState(Type type) =>
            _states.ContainsKey(type);

        public TState GetState<TState>() where TState : class, IState =>
            _states[typeof(TState)] as TState;
    }
}