using UnityEngine;

namespace Cubra
{
    public class Platform : CollisionObjects
    {
        /// <summary>
        /// Действия при касании персонажа с коллайдером
        /// </summary>
        /// <param name="character">персонаж</param>
        public override void ActionsOnEnter(Character character)
        {
            if (character.Life)
            {
                // Назначаем платформу родительской для персонажа
                character.Transform.parent = Transform.parent;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            var character = collision.gameObject.GetComponent<Character>();

            if (character)
            {
                // Убираем родительский объект персонажа
                character.Transform.parent = null;
            }
        }
    }
}