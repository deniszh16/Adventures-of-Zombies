using Services.Sound;
using UnityEngine;

namespace Logic.Obstacles
{
    public class Trap : SharpObstacles
    {
        [Header("Компоненты капкана")]
        [SerializeField] private PlayingSound _playingSound;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Animator _animator;

        protected override void OnTriggerEnter2D(Collider2D col)
        {
            _playingSound.PlaySound();
            _animator.enabled = true;
            _animator.Rebind();
            
            base.OnTriggerEnter2D(col);
            _spriteRenderer.sortingOrder += 2;
        }
    }
}