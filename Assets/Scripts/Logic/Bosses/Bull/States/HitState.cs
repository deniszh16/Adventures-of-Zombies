using StateMachine.States;
using UnityEngine;

namespace Logic.Bosses.Bull.States
{
    public class HitState : BaseStates
    {
        [Header("Компоненты босса")]
        [SerializeField] private Animator _animator;
        
        private static readonly int AttackAnimation = Animator.StringToHash("Attack");
        
        public override void Enter() =>
            _animator.SetTrigger(AttackAnimation);

        public override void Exit()
        {
        }
    }
}