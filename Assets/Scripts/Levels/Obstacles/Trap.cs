using UnityEngine;

namespace Cubra
{
    public class Trap : SharpObstacles
    {
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