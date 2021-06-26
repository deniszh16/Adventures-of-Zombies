using System.Collections;
using UnityEngine;

namespace Cubra
{
    public class Stones : MonoBehaviour
    {
        // Начальная позиция объекта
        private Vector2 _position;

        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _position = transform.position;
        }

        private void OnEnable()
        {
            _ = StartCoroutine(RestoreObject());
        }

        /// <summary>
        /// Восстановление объекта с изменением позиции
        /// </summary>
        private IEnumerator RestoreObject()
        {
            yield return new WaitForSeconds(5f);
            _rigidbody.velocity *= 0;

            transform.position = new Vector2(_position.x + Random.Range(-3, 3), _position.y + Random.Range(-2, 2));
            gameObject.SetActive(false);
        }
    }
}