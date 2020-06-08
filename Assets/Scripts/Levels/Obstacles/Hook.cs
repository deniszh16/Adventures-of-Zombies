using UnityEngine;

namespace Cubra
{
    public class Hook : CollisionObjects
    {
        // Занят ли крюк
        private bool _busy;

        /// <summary>
        /// Действия при касании персонажа с коллайдером
        /// </summary>
        /// <param name="character">персонаж</param>
        public override void ActionsOnEnter(Character character)
        {
            if (character.Life)
            {
                // Активируем вис на крюке
                Main.Instance.CharacterController.IsHook = true;
                // Назначаем крюк родительским объектом для персонажа
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
                        // Фиксируем персонажа на крюке
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
                // Сбрасываем родительский объект для персонажа
                character.transform.parent = null;
                // Восстанавливаем стандартную гравитацию персонажа
                character.Rigidbody.gravityScale = 1.5f;
                // Восстанавливаем скорость
                character.Speed = 8.5f;
            }
        }
    }
}