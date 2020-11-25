using UnityEngine;

namespace Cubra
{
    public class Trampoline : ReboundObject
    {
        public override void ActionsOnEnter(Character character)
        {
            if (character.Life)
            {
                character.Rigidbody.velocity = Vector2.zero;
                character.Rigidbody.AddForce(_direction * _force, ForceMode2D.Impulse);

                _playingSound.PlaySound();
                _animator.enabled = true;
                _animator.Rebind();
            }
        }
    }
}