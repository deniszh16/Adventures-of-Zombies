using System.Collections;
using Logic.Camera;
using StateMachine.States;
using UnityEngine;

namespace Logic.Bosses.Yeti.States
{
    public class AttackState : BaseStates
    {
        [Header("Компоненты босса")]
        [SerializeField] private Yeti _yeti;
        [SerializeField] private Animator _animator;
        
        [Header("Падающие камни")]
        [SerializeField] private GameObject[] _stones;
        
        [Header("Прочие компоненты")]
        [SerializeField] protected GameCamera _gameCamera;
        
        private static readonly int SmashAnimation = Animator.StringToHash("Smash");
        
        public override void Enter()
        {
            _animator.SetTrigger(SmashAnimation);
            _ = StartCoroutine(Rockfall(1f));
            _yeti.SetDirection(spriteReversal: true);
            _yeti.SetQuantityRun(2, 4);
            _yeti.DefineNextState();
        }
        
        private IEnumerator Rockfall(float pause)
        {
            yield return new WaitForSeconds(pause);
            _gameCamera.ShakeCamera(duration: 0.7f, amplitude: 1.8f, frequency: 1.6f);
            
            for (int i = 0; i < _stones.Length; i++)
                _stones[i].SetActive(true);
        }

        public override void Exit()
        {
        }
    }
}