using StateMachine.States;
using UnityEngine;

namespace Logic.Bosses.Bull.States
{
    public class IdleState : BaseStates
    {
        [Header("Компоненты босса")]
        [SerializeField] private Animator _animator;
        
        private static readonly int IdleAnimation = Animator.StringToHash("Idle");
        
        public override void Enter() =>
            _animator.Play(IdleAnimation);

        public override void Exit()
        {
        }
    }
}