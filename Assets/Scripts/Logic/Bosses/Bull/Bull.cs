using System.Collections;
using Logic.Bosses.Bull.States;
using Logic.Characters;
using Logic.Obstacles;
using UnityEngine;

namespace Logic.Bosses.Bull
{
    public class Bull : Boss
    {
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.TryGetComponent(out Character character))
            {
                _bossStateMachine.Enter<HitState>();
                
                character.DamageToCharacter();
                character.ShowDeathAnimation();
                character.DisableCollider();
                character.PlayDeadSound();
            }

            if (col.gameObject.GetComponent<Wall>())
            {
                _gameCamera.ShakeCamera(duration: 0.6f, amplitude: 2.1f, frequency: 1.9f);
                _bossStateMachine.Enter<StuporState>();
            }
        }

        protected override void GoToStartState() =>
            _bossStateMachine.Enter<StartingState>();

        public override void DefineNextState() =>
            _ = StartCoroutine(DefineNextStateCoroutine());

        protected override void GoToIdleState()
        {
            StopAllCoroutines();
            _bossStateMachine.Enter<IdleState>();
        }

        private IEnumerator DefineNextStateCoroutine()
        {
            if (Health > 0)
            {
                float pause = Random.Range(2.2f, 3.6f);
                yield return new WaitForSeconds(pause);
                
                if (QuantityRuns > 0)
                {
                    _bossStateMachine.Enter<RunState>();
                }
                else
                {
                    _bossStateMachine.Enter<AttackState>();
                }
            }
            else
            {
                _bossStateMachine.Enter<DieState>();
            }
        }
    }
}