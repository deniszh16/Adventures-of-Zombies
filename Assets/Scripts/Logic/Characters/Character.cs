using System;
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
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private PolygonCollider2D _collider;
        [SerializeField] private CharacterSounds _characterSounds;

        public bool Life { get; private set; } = true;

        public event Action CharacterDied;
        
        public Vector3 RespawnPosition { get; set; }

        private static readonly int IdleAnimation = Animator.StringToHash("Idle");
        private static readonly int DeadAnimation = Animator.StringToHash("Dead");
        private static readonly int DrowningAnimation = Animator.StringToHash("Drowning");

        private const int InitialLayer = 10;

        private GameStateMachine _gameStateMachine;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine) =>
            _gameStateMachine = gameStateMachine;

        public void DamageToCharacter()
        {
            Life = false;
            CharacterDied?.Invoke();
            
            _rigidbody.velocity = Vector2.zero;
            _gameStateMachine.Enter<LosingState>();
        }

        public void CharacterRespawn()
        {
            Life = true;
            _rigidbody.velocity = Vector2.zero;
            transform.position = RespawnPosition;
            _animator.Play(IdleAnimation);
            _spriteRenderer.sortingOrder = InitialLayer;
        }

        public void ShowDeathAnimation() =>
            _animator.SetTrigger(DeadAnimation);

        public void ShowSinkingAnimation() =>
            _animator.SetTrigger(DrowningAnimation);

        public void DisableCollider() =>
            _collider.isTrigger = true;

        public void PlayDeadSound()
        {
            _characterSounds.SetSound(Sounds.Dead);
            _characterSounds.PlaySound();
        }
    }
}