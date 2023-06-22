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
        private IState _activeState;

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
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;
            return state;
        }

        public bool CheckState(Type type) =>
            _states.ContainsKey(type);

        public TState GetState<TState>() where TState : class, IState =>
            _states[typeof(TState)] as TState;
    }
}