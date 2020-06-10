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
            _playingSound.PlaySound();
            _animator.enabled = true;
            _animator.Rebind();

            base.ActionsOnEnter(character);

            _spriteRenderer.sortingOrder += 2;
        }
    }
}