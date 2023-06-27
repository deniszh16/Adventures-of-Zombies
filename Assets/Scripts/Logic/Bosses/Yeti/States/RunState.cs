using StateMachine.States;
using UnityEngine;

namespace Logic.Bosses.Yeti.States
{
    public class RunState : BaseStates
    {
        [Header("Компоненты босса")]
        [SerializeField] private Yeti _yeti;
        [SerializeField] private Animator _animator;
        
        [Header("Препятствия уровня")]
        [SerializeField] private GameObject _saw;
        [SerializeField] private GameObject _slowdown;
        
        private static readonly int RunAnimation = Animator.StringToHash("Run");
        
        public override void Enter()
        {
            _yeti.SetBossSpeed(14, 16);
            _yeti.QuantityRuns -= 1;
            _animator.SetTrigger(RunAnimation);
            _yeti.SetDirection(spriteReversal: true);
            _yeti.ColliderOffset();
            _yeti.RunningActivity = true;

            ShowObstacles();
        }
        
        private void ShowObstacles()
        {
            if (_yeti.Health <= 10 && _saw.activeInHierarchy == false)
                _saw.SetActive(true);
            
            if (_yeti.Health <= 5 && _slowdown.activeInHierarchy == false)
                _slowdown.SetActive(true);
        }

        public override void Exit() =>
            _yeti.RunningActivity = false;
    }
}