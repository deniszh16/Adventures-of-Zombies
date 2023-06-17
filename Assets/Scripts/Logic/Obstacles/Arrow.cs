using System.Collections;
using UnityEngine;

namespace Logic.Obstacles
{
    public class Arrow : MonoBehaviour
    {
        [Header("Компоненты стрелы")]
        [SerializeField] private BoxCollider2D _boxCollider;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private FixedJoint2D _joint;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private bool _flight;
        
        private float _speed;
        private const float StandardSpeed = 15f;
        private const string SurfaceLayer = "Surface";
        
        private int _sortingOrder;
        
        private Vector3 _direction;

        private void Awake() =>
            _sortingOrder = _spriteRenderer.sortingOrder;

        private void OnEnable()
        {
            _direction = transform.TransformDirection(Vector3.down);
            
            _flight = true;
            _boxCollider.enabled = true;
            _spriteRenderer.sortingOrder = _sortingOrder;
            _speed = StandardSpeed;
        }

        private void FixedUpdate()
        {
            if (_flight)
                _rigidbody.MovePosition(transform.position + _direction * (_speed * Time.fixedDeltaTime));
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == LayerMask.GetMask(SurfaceLayer) >> 5)
            {
                _boxCollider.enabled = false;
                _spriteRenderer.sortingOrder--;

                if (col.TryGetComponent(out Rigidbody2D physics))
                {
                    _rigidbody.mass = 0;
                    _joint.connectedBody = physics;
                }

                if (gameObject.activeInHierarchy)
                    _ = StartCoroutine(StopFlight(0.03f, physics));

                return;
            }

            if (col.gameObject.GetComponent<SharpObstacles>())
            {
                gameObject.SetActive(false);
                return;
            }

            if (col.gameObject.GetComponent<River.River>())
            {
                _speed /= 2;
            }
        }

        private IEnumerator StopFlight(float seconds, bool fixation)
        {
            yield return new WaitForSeconds(seconds);
            _flight = false;
            
            if (fixation)
                _joint.enabled = true;
            
            _ = StartCoroutine(TurnOffObject());
        }

        private IEnumerator TurnOffObject()
        {
            yield return new WaitForSeconds(1.5f);
            transform.localEulerAngles = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}