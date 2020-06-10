using UnityEngine;

namespace Cubra
{
    public class Hook : CollisionObjects
    {
        // Касание крюка
        private bool _busy;

        /// <summary>
        /// Действия при касании персонажа с коллайдером
        /// </summary>
        /// <param name="character">персонаж</param>
        public override void ActionsOnEnter(Character character)
        {
            if (character.Life)
            {
                Main.Instance.CharacterController.IsHook = true;
                character.Transform.parent = transform;
                _busy = true;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (_busy)
            {
                // Получаем компонент персонажа у касающегося объекта
                var character = collision.GetComponent<Character>();

                if (character.Life)
                {
                    // Если персонаж находится в указанных пределах крюка
                    if (character.Transform.localPosition.x < 0.5f && character.Transform.localPosition.y < -0.35f)
                    {
                        Main.Instance.CharacterController.HangOnHook();
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