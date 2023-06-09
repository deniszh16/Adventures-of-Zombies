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

        private int _currentPoint = 0;

        private void Update()
        {
            if (_points.Length > 0 && transform.position != _points[_currentPoint])
            {
                transform.position =
                    Vector3.MoveTowards(transform.position, _points[_currentPoint], _speed * Time.deltaTime);
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