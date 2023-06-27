using System.Collections;
using Logic.Characters;
using StateMachine.States;
using UnityEngine;

namespace Logic.Bosses.Yeti.States
{
    public class HitState : BaseStates
    {
        [Header("Компоненты босса")]
        [SerializeField] private Yeti _yeti;
        [SerializeField] private Animator _animator;
        
        [Header("Персонаж")]
        [SerializeField] private Character _character;
        
        private static readonly int PunchAnimation = Animator.StringToHash("Punch");
        
        public override void Enter()
        {
            _animator.SetTrigger(PunchAnimation);
            _ = StartCoroutine(HitCharacter());
            _yeti.RunningActivity = false;
            _yeti.DefineNextState();
            _yeti.SetDirection(spriteReversal: true);
            _yeti.ColliderOffset();
        }
        
        private IEnumerator HitCharacter()
        {
            yield return new WaitForSeconds(0.5f);
            if (Mathf.Abs(transform.position.x - _character.transform.position.x) < 6 &&
                Mathf.Abs(transform.position.y - _character.transform.position.y) < 2.8f)
            {
                _character.DamageToCharacter();
                _character.ShowDeathAnimation();
            }
        }

        public override void Exit()
        {
        }
    }
}