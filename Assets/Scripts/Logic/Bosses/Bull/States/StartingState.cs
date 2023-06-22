using System.Collections;
using StateMachine;
using StateMachine.States;
using UnityEngine;

namespace Logic.Bosses.Bull.States
{
    public class StartingState : BaseStates
    {
        [Header("Компоненты босса")]
        [SerializeField] private Boss _boss;
        [SerializeField] private BossStateMachine _bossStateMachine;
        
        public override void Enter()
        {
            _boss.SetQuantityRun(2, 4);
            _ = StartCoroutine(StartRunningBull(pause: 0.3f));
        }

        private IEnumerator StartRunningBull(float pause)
        {
            yield return new WaitForSeconds(pause);
            _bossStateMachine.Enter<RunState>();
        }

        public override void Exit()
        {
        }
    }
}