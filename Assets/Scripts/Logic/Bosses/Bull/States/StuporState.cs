using StateMachine.States;
using UnityEngine;

namespace Logic.Bosses.Bull.States
{
    public class StuporState : BaseStates
    {
        [Header("Компоненты босса")]
        [SerializeField] private Boss _boss;
        [SerializeField] protected Rigidbody2D _rigbody;
        [SerializeField] protected Animator _animator;
        [SerializeField] protected GameObject _stunEffect;
        
        private static readonly int IdleAnimation = Animator.StringToHash("Idle");
        
        public override void Enter()
        {
            _boss.DecreaseHealth(1);
            _rigbody.AddForce(_boss.Direction * -3.5f, ForceMode2D.Impulse);
            _stunEffect.SetActive(true);
            _stunEffect.transform.localPosition = new Vector2(3.2f * _boss.Direction.x, _stunEffect.transform.localPosition.y);
            _animator.SetTrigger(IdleAnimation);
            _boss.DefineNextState();
        }

        public override void Exit() =>
            _stunEffect.SetActive(false);
    }
}