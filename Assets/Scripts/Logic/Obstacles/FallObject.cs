using System.Collections;
using Logic.Characters;
using UnityEngine;

namespace Logic.Obstacles
{
    public class FallObject : MonoBehaviour
    {
        [Header("Время до падения")]
        [SerializeField] private float _seconds;
        
        [Header("Компоненты объекта")]
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private Transform _transform;
        
        private int _sortingOrder;
        private Vector3 _position;

        private void Awake() =>
            _transform = transform;

        private void Start()
        {
            _sortingOrder = _spriteRenderer.sortingOrder; 
            _position = transform.position;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.GetComponent<Character>())
                _ = StartCoroutine(ObjectFall());
        }
        
        private IEnumerator ObjectFall()
        {
            yield return new WaitForSeconds(_seconds);
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.GetComponent<River.River>())
            {
                _spriteRenderer.sortingOrder = 0;
                return;
            }

            if (col.GetComponent<SharpObstacles>())
            {
                _spriteRenderer.enabled = false;
                _ = StartCoroutine(RestoreObject(seconds: 1.5f));
            }
        }
        
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<River.River>())
                _ = StartCoroutine(RestoreObject(seconds: 1f));
        }
        
        private IEnumerator RestoreObject(float seconds)
        {
            yield return new WaitForSeconds(seconds); 
            _spriteRenderer.enabled = true; 
            _spriteRenderer.sortingOrder = _sortingOrder;

            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
            _rigidbody.velocity = Vector2.zero;
            
            _transform.position = _position;
        }
    }
}