using System.Collections;
using StateMachine.States;
using UnityEngine;

namespace Logic.Bosses.Yeti.States
{
    public class ThrowState : BaseStates
    {
        [Header("Компоненты босса")]
        [SerializeField] private Yeti _yeti;
        [SerializeField] private Animator _animator;
        
        [Header("Камень для броска")]
        [SerializeField] private GameObject _stone;
        [SerializeField] private Rigidbody2D _rigidbody;

        private static readonly int ThrowAnimation = Animator.StringToHash("Throw");
        
        public override void Enter()
        {
            _animator.SetTrigger(ThrowAnimation);
            _ = StartCoroutine(ThrowAStone());
            _yeti.SetDirection(spriteReversal: true);
            _yeti.ColliderOffset();
            _yeti.DefineNextState();
        }

        private IEnumerator ThrowAStone()
        {
            yield return new WaitForSeconds(0.6f);
            _stone.SetActive(true);
            _stone.transform.localPosition = new Vector2(3.4f * _yeti.Direction.x, 0.4f);
            _rigidbody.AddForce(new Vector2(Random.Range(32, 35) * _yeti.Direction.x, 1.5f), ForceMode2D.Impulse);
            yield return new WaitForSeconds(3f);
            _rigidbody.velocity *= 0;
            _stone.SetActive(false);
        }

        public override void Exit()
        {
        }
    }
}