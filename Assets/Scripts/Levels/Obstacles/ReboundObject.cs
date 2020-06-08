using UnityEngine;

namespace Cubra
{
    public class ReboundObject : CollisionObjects
    {
        [Header("Вектор отскока")]
        [SerializeField] protected Vector2 _direction;

        [Header("Сила отскока")]
        [SerializeField] protected float _force;

        // Ссылки на компоненты
        protected Animator _animator;
        protected PlayingSound _playingSound;

        protected override void Awake()
        {
            base.Awake();

            _animator = InstanseObject.GetComponent<Animator>();
            _playingSound = InstanseObject.GetComponent<PlayingSound>();
        }

        /// <summary>
        /// Действия при касании персонажа с коллайдером
        /// </summary>
        /// <param name="character">персонаж</param>
        public override void ActionsOnEnter(Character character)
        {
            // Создаем отскок персонажа в указанном направлении
            character.Rigidbody.AddForce(_direction * _force, ForceMode2D.Impulse);

            // Воспрозводим звук
            _playingSound.PlaySound();

            // Активируем и перезапускаем анимацию
            _animator.enabled = true;
            _animator.Rebind();
        }
    }
}