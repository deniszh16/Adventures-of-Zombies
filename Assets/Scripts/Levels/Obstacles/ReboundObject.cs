using UnityEngine;

namespace Cubra
{
    public class ReboundObject : CollisionObjects
    {
        [Header("Вектор отскока")]
        [SerializeField] protected Vector2 _direction;

        [Header("Сила отскока")]
        [SerializeField] protected float _force;

        protected Animator _animator;
        protected PlayingSound _playingSound;

        protected override void Awake()
        {
            base.Awake();

            _animator = InstanseObject.GetComponent<Animator>();
            _playingSound = InstanseObject.GetComponent<PlayingSound>();
        }
        
        public override void ActionsOnEnter(Character character)
        {
            character.Rigidbody.AddForce(_direction * _force, ForceMode2D.Impulse);

            _playingSound.PlaySound();
            _animator.enabled = true;
            _animator.Rebind();
        }
    }
}