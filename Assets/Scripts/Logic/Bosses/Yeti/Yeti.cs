using System.Collections;
using Logic.Bosses.Yeti.States;
using Logic.Characters;
using Logic.Obstacles;
using UnityEngine;

namespace Logic.Bosses.Yeti
{
    public class Yeti : Boss
    {
        [Header("Коллайдер руки")]
        [SerializeField] private CapsuleCollider2D _yetiFist;
        
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
                float pause = Random.Range(2, 3.2f);
                yield return new WaitForSeconds(pause);

                if (_bossStateMachine.ActiveState is AttackState)
                {
                    _bossStateMachine.Enter<ThrowState>();
                }
                else
                {
                    if (QuantityRuns > 0)
                    {
                        _bossStateMachine.Enter<RunState>();
                    }
                    else
                    {
                        _bossStateMachine.Enter<AttackState>();
                    }
                }
            }
            else
            {
                _bossStateMachine.Enter<DieState>();
            }
        }
        
        public void ColliderOffset()
        {
            float x = Direction.x < 0 ? -2.7f : 2.7f;
            _yetiFist.offset = new Vector2(x, -0.2f);
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Character character))
            {
                if (_bossStateMachine.ActiveState is StuporState)
                {
                    character.DamageToCharacter();
                    character.ShowDeathAnimation();
                }
            }

            if (collision.gameObject.GetComponent<Wall>())
            {
                if (_bossStateMachine.ActiveState is StuporState == false)
                {
                    _gameCamera.ShakeCamera(duration: 0.6f, amplitude: 2.1f, frequency: 1.9f);
                    _bossStateMachine.Enter<StuporState>();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.GetComponent<Character>())
            {
                if (_bossStateMachine.ActiveState is StuporState == false)
                    _bossStateMachine.Enter<HitState>();
            }
        }
    }
}