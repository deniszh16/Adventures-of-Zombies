using StateMachine;
using StateMachine.States;
using UnityEngine;
using Zenject;

namespace Logic.Characters
{
    public class Character : MonoBehaviour
    {
        [Header("Компоненты персонажа")]
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private PolygonCollider2D _collider;
        [SerializeField] private CharacterSounds _characterSounds;

        public bool Life { get; private set; } = true;

        private static readonly int DeadAnimation = Animator.StringToHash("Dead");
        private static readonly int DrowningAnimation = Animator.StringToHash("Drowning");

        private GameStateMachine _gameStateMachine;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine) =>
            _gameStateMachine = gameStateMachine;

        public void DamageToCharacter()
        {
            Life = false;
            
            _rigidbody.velocity = Vector2.zero;
            _gameStateMachine.Enter<LosingState>();
        }

        public void ShowDeathAnimation() =>
            _animator.SetTrigger(DeadAnimation);

        public void ShowSinkingAnimation() =>
            _animator.SetTrigger(DrowningAnimation);

        public void DisableCollider() =>
            _collider.isTrigger = true;

        public void CharacterRebound() =>
            _rigidbody.AddForce(new Vector2(Random.Range(-135, -100) * transform.localScale.x, Random.Range(160, 190)));

        public void PlayJumpSound()
        {
            _characterSounds.SetSound(Sounds.Dead);
            _characterSounds.PlaySound();
        }
    }
}