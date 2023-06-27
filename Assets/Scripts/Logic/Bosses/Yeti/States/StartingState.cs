using System.Collections;
using StateMachine;
using StateMachine.States;
using UnityEngine;

namespace Logic.Bosses.Yeti.States
{
    public class StartingState : BaseStates
    {
        [Header("Компоненты босса")]
        [SerializeField] private Boss _boss;
        [SerializeField] private BossStateMachine _bossStateMachine;


        public override void Enter() =>
            _ = StartCoroutine(StartAttack(0.5f));

        private IEnumerator StartAttack(float pause)
        {
            yield return new WaitForSeconds(pause);
            _bossStateMachine.Enter<ThrowState>();
            _boss.SetQuantityRun(2, 4);
        }

        public override void Exit()
        {
        }
    }
}