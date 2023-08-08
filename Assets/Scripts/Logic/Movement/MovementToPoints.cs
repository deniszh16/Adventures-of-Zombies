using UnityEngine;

namespace Logic.Movement
{
    public class MovementToPoints : MonoBehaviour
    {
        [Header("Точки для перемещения")]
        [SerializeField] protected Vector3[] _points;

        [Header("Скорость движения")]
        [SerializeField] protected float _speed;
        
        [Header("Компонент рендера")]
        [SerializeField] protected SpriteRenderer _spriteRenderer;

        [Header("Разворот спрайта")]
        [SerializeField] protected bool _turningSprite;

        private Transform _transform;
        private int _currentPoint = 0;

        private void Awake() =>
            _transform = transform;

        private void Update()
        {
            if (_points.Length > 0 && _transform.position != _points[_currentPoint])
            {
                _transform.position =
                    Vector3.MoveTowards(_transform.position, _points[_currentPoint], _speed * Time.deltaTime);
            }
            else
            {
                if (_turningSprite)
                    _spriteRenderer.flipX = !_spriteRenderer.flipX;

                _currentPoint = _currentPoint < _points.Length - 1 ? ++_currentPoint : 0;
            }
        }
    }
}