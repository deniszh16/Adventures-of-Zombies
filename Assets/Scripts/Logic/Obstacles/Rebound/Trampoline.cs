using Logic.Characters;
using UnityEngine;

namespace Logic.Obstacles.Rebound
{
    public class Trampoline : ReboundObject
    {
        protected override void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.TryGetComponent(out Character character))
            {
                if (character.Life)
                {
                    Rigidbody2D rigidbody = character.GetComponent<Rigidbody2D>();
                    rigidbody.velocity = Vector2.zero;
                    rigidbody.AddForce(_direction * _force, ForceMode2D.Impulse);
                    
                    _playingSound.PlaySound();
                    _animator.enabled = true;
                    _animator.Rebind();
                }
            }
        }
    }
}