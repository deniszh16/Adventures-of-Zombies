using UnityEngine;

namespace Cubra
{
    public abstract class CollisionObjects : BaseObjects
    {
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            // Получаем компонент персонажа у конувшегося объекта
            var character = collision.GetComponent<Character>();

            if (character)
                ActionsOnEnter(character);
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            // Получаем компонент персонажа у конувшегося объекта
            var character = collision.gameObject.GetComponent<Character>();

            if (character)
                ActionsOnEnter(character);
        }

        /// <summary>
        /// Действия при касании персонажа с коллайдером
        /// </summary>
        /// <param name="character">персонаж</param>
        public abstract void ActionsOnEnter(Character character);
    }
}