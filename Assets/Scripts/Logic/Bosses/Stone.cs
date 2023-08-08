using System.Collections;
using UnityEngine;

namespace Logic.Bosses
{
    public class Stone : MonoBehaviour
    {
        [Header("Физический компонент")]
        [SerializeField] private Rigidbody2D _rigidbody;

        private Transform _transform;
        private Vector3 _position;
        
        private void Start()
        {
            _transform = transform;
            _position = _transform.position;
        }

        private void OnEnable() =>
            _ = StartCoroutine(RestoreObject());

        private IEnumerator RestoreObject()
        {
            yield return new WaitForSeconds(3f);
            _rigidbody.velocity = Vector2.zero;

            _transform.position = new Vector2(_position.x + Random.Range(-3, 3), _position.y + Random.Range(-2, 2));
            gameObject.SetActive(false);
        }
    }
}