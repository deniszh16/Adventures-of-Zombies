using StateMachine.States;
using UnityEngine;

namespace Logic.Bosses.Yeti.States
{
    public class StuporState : BaseStates
    {
        [Header("Компоненты босса")]
        [SerializeField] private Yeti _yeti;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Animator _animator;
        
        [Header("Эффект оглушения")]
        [SerializeField] protected GameObject _stunEffect;
        
        private static readonly int IdleAnimation = Animator.StringToHash("Idle");
        
        public override void Enter()
        {
            _yeti.DecreaseHealth(1);
            _rigidbody.AddForce(_yeti.Direction * -18.5f, ForceMode2D.Impulse);
            _animator.SetTrigger(IdleAnimation);
            
            _stunEffect.SetActive(true);
            _stunEffect.transform.localPosition = new Vector2(2.25f * _yeti.Direction.x, _stunEffect.transform.localPosition.y);

            _yeti.DefineNextState();
        }

        public override void Exit() =>
            _stunEffect.SetActive(false);
    }
}