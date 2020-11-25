using UnityEngine;

namespace Cubra
{
    public class Platform : CollisionObjects
    {
        public override void ActionsOnEnter(Character character)
        {
            if (character.Life)
                character.Transform.parent = Transform.parent;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            var character = collision.gameObject.GetComponent<Character>();

            if (character)
                character.Transform.parent = null;
        }
    }
}