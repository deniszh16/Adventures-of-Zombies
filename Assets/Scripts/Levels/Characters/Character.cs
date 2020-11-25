using UnityEngine;

namespace Cubra
{
    public class Character : BaseObjects
    {
        // Жизнь персонажа
        public bool Life { get; set; } = true;

        [Header("Высота прыжка")]
        [SerializeField] private float _jump;

        public float Jump => _jump;

        [Header("Скорость движения")]
        [SerializeField] private float _speed;

        public float Speed { get => _speed; set => _speed = value; }

        public enum Animations { Idle, Run, Jump, Dead, Hang }

        [Header("Звуки персонажа")]
        [SerializeField] private AudioClip[] _audioClips;

        public enum Sounds { Dead, Brain, Coin }

        // Ссылки на компоненты персонажа
        public Rigidbody2D Rigidbody { get; private set; }
        public ParticleSystem BloodEffect { get; private set; }
        public PolygonCollider2D PolygonCollider { get; private set; }
        public AudioSource AudioSource { get; private set; }
        public PlayingSound PlayingSound { get; private set; }
        private Animator _animator;

        protected override void Awake()
        {
            base.Awake();

            Rigidbody = InstanseObject.GetComponent<Rigidbody2D>();
            BloodEffect = InstanseObject.GetComponentInChildren<ParticleSystem>();
            PolygonCollider = InstanseObject.GetComponent<PolygonCollider2D>();
            AudioSource = InstanseObject.GetComponent<AudioSource>();
            PlayingSound = InstanseObject.GetComponent<PlayingSound>();
            _animator = InstanseObject.GetComponent<Animator>();
        }

        /// <summary>
        /// Установка анимации персонажа
        /// </summary>
        /// <param name="animation">анимация</param>
        public void SetAnimation(Animations animation)
        {
            _animator.SetInteger("State", (int)animation);
        }

        /// <summary>
        /// Установка звука персонажа
        /// </summary>
        /// <param name="sound">звук</param>
        public void SetSound(Sounds sound)
        {
            AudioSource.clip = _audioClips[(int)sound];
        }
    }
}