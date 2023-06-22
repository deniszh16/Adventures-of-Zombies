using System.Collections;
using Logic.Camera;
using StateMachine.States;
using UnityEngine;

namespace Logic.Bosses.Bull.States
{
    public class AttackState : BaseStates
    {
        [Header("Компоненты босса")]
        [SerializeField] private Boss _boss;
        [SerializeField] private Animator _animator;

        [Header("Падающие камни")]
        [SerializeField] private GameObject[] _stones;
        
        [Header("Прочие компоненты")]
        [SerializeField] protected GameCamera _gameCamera;
        
        private static readonly int AttackAnimation = Animator.StringToHash("Attack");

        public override void Enter()
        {
            _boss.SetDirection(false);
            _animator.SetTrigger(AttackAnimation);

            _ = StartCoroutine(Rockfall(0.55f));
            
            _boss.SetQuantityRun(2,4);
            _boss.DefineNextState();
        }
        
        private IEnumerator Rockfall(float pause)
        {
            yield return new WaitForSeconds(pause);
            _gameCamera.ShakeCamera(duration: 0.7f, amplitude: 1.8f, frequency: 1.6f);
            
            for (int i = 0; i < _stones.Length; i++)
                _stones[i].SetActive(true);
        }

        public override void Exit() =>
            StopAllCoroutines();
    }
}