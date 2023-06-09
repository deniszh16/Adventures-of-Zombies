using UnityEngine;

namespace StateMachine.States
{
    public abstract class BaseStates : MonoBehaviour, IState
    {
        public abstract void Enter();
        public abstract void Exit();
    }
}