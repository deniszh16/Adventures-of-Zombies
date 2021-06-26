using UnityEngine;

namespace Cubra
{
    public class Hook : CollisionObjects
    {
        // Касание крюка
        private bool _busy;
        
        public override void ActionsOnEnter(Character character)
        {
            if (character.Life)
            {
                GameManager.Instance.CharacterController.IsHanging = true;
                character.Transform.parent = transform;
                _busy = true;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (_busy)
            {
                var character = collision.GetComponent<Character>();

                if (character.Life)
                {
                    // Если персонаж находится в указанных пределах крюка
                    if (character.Transform.localPosition.x < 0.5f && character.Transform.localPosition.y < -0.35f)
                    {
                        GameManager.Instance.CharacterController.HangOnHook();
                        _busy = false;
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var character = collision.GetComponent<Character>();

            if (character.Life)
            {
                character.transform.parent = null;
                character.Rigidbody.gravityScale = 1.5f;
                character.Speed = 8.5f;
            }
        }
    }
}