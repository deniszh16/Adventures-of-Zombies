using System.Collections;
using UnityEngine;

namespace Cubra
{
    public class FallObject : CollisionObjects
    {
        [Header("Время до падения")]
        [SerializeField] private float _seconds;

        // Стандартный слой объекта
        private int _sortingOrder;

        // Начальная позиция объекта
        private Vector3 _position;

        // Ссылка на компонент
        private Rigidbody2D _rigidbody;

        protected override void Awake()
        {
            base.Awake();
            _rigidbody = InstanseObject.GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            // Получаем слой объекта
            _sortingOrder = SpriteRenderer.sortingOrder;
            // Получаем позицию объекта
            _position = Transform.position;
        }

        /// <summary>
        /// Действия при касании персонажа с коллайдером
        /// </summary>
        /// <param name="character">персонаж</param>
        public override void ActionsOnEnter(Character character)
        {
            _ = StartCoroutine(ObjectFall());
        }

        /// <summary>
        /// Падение объекта
        /// </summary>
        private IEnumerator ObjectFall()
        {
            yield return new WaitForSeconds(_seconds);
            // Активируем динамическую физику объекта
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            // Получаем водный компонент 
            var river = collision.GetComponent<River>();

            if (river)
            {
                // Переносим объект на нулевой слой
                SpriteRenderer.sortingOrder = 0;
                return;
            }

            // Получаем компонент препятствия
            var obstacle = collision.GetComponent<SharpObstacles>();

            if (obstacle)
            {
                // Скрываем спрайт
                SpriteRenderer.enabled = false;

                _ = StartCoroutine(RestoreObject(2f));
            }    
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var river = collision.GetComponent<River>();

            if (river)
            {
                _ = StartCoroutine(RestoreObject(1f));
            }
        }

        /// <summary>
        /// Восстановление объекта
        /// </summary>
        /// <param name="seconds">пауза перед восстановлением</param>
        private IEnumerator RestoreObject(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            // Отображаем спрайт
            SpriteRenderer.enabled = true;
            // Восстанавливаем слой объекта
            SpriteRenderer.sortingOrder = _sortingOrder;

            // Восстанавливаем физику объекта
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
            _rigidbody.velocity = Vector2.zero;

            // Восстанавливаем позицию
            Transform.position = _position;
        }
    }
}