using UnityEngine;

namespace Cubra
{
    public class Trap : SharpObstacles
    {
        // Ссылки на компоненты
        private PlayingSound _playingSound;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;

        protected override void Awake()
        {
            base.Awake();
            _playingSound = InstanseObject.GetComponent<PlayingSound>();
            _spriteRenderer = InstanseObject.GetComponent<SpriteRenderer>();
            _animator = InstanseObject.GetComponent<Animator>();
        }

        /// <summary>
        /// Действия при касании персонажа с коллайдером
        /// </summary>
        /// <param name="character">персонаж</param>
        public override void ActionsOnEnter(Character character)
        {
            // Воспроизводим звук
            _playingSound.PlaySound();

            // Перезапускаем анимацию
            _animator.enabled = true;
            _animator.Rebind();

            base.ActionsOnEnter(character);

            // Повышаем слой объекта
            _spriteRenderer.sortingOrder += 2;
        }
    }
}