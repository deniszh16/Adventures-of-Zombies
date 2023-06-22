using StateMachine.States;
using UnityEngine;

namespace Logic.Bosses.Bull.States
{
    public class RunState : BaseStates
    {
        [Header("Компоненты босса")]
        [SerializeField] private Boss _boss;
        [SerializeField] private Animator _animator;

        [Header("Препятствия уровня")]
        [SerializeField] private GameObject _spikes;
        [SerializeField] private GameObject _arrows;

        private static readonly int RunAnimation = Animator.StringToHash("Run");

        public override void Enter()
        {
            _boss.SetBossSpeed(14, 18);
            _boss.SetDirection(spriteReversal: false);
            _boss.QuantityRuns -= 1;
            _animator.SetTrigger(RunAnimation);
            _boss.RunningActivity = true;
            
            ShowObstacles();
        }

        private void ShowObstacles()
        {
            if (_boss.Health <= 10 && _spikes.activeInHierarchy == false)
                _spikes.SetActive(true);
            
            if (_boss.Health <= 5 && _arrows.activeInHierarchy == false)
                _arrows.SetActive(true);
        }

        public override void Exit() =>
            _boss.RunningActivity = false;
    }
}