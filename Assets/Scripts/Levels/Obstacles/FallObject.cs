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

        private Rigidbody2D _rigidbody;

        protected override void Awake()
        {
            base.Awake();
            _rigidbody = InstanseObject.GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _sortingOrder = SpriteRenderer.sortingOrder;
            _position = Transform.position;
        }
        
        public override void ActionsOnEnter(Character character)
        {
            _ = StartCoroutine(ObjectFall());
        }

        /// <summary>
        /// Падение объекта через указанное время
        /// </summary>
        private IEnumerator ObjectFall()
        {
            yield return new WaitForSeconds(_seconds);
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<River>())
            {
                SpriteRenderer.sortingOrder = 0;
                return;
            }

            if (collision.GetComponent<SharpObstacles>())
            {
                SpriteRenderer.enabled = false;
                _ = StartCoroutine(RestoreObject(2f));
            }    
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<River>())
                _ = StartCoroutine(RestoreObject(1f));
        }

        /// <summary>
        /// Восстановление объекта
        /// </summary>
        /// <param name="seconds">пауза перед восстановлением</param>
        private IEnumerator RestoreObject(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            SpriteRenderer.enabled = true;
            SpriteRenderer.sortingOrder = _sortingOrder;

            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
            _rigidbody.velocity = Vector2.zero;

            Transform.position = _position;
        }
    }
}