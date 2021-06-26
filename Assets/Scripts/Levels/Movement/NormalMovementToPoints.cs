using UnityEngine;

namespace Cubra
{
    public class NormalMovementToPoints : MovementToPoints
    {
        private void Update()
        {
            // Если текущая позиция не равна позиции активной точки
            if (_points.Length > 0 && Transform.position != _points[_currentPoint])
            {
                Transform.position = Vector3.MoveTowards(Transform.position, _points[_currentPoint], _speed * Time.deltaTime);
            }
            else
            {
                if (_turningSprite)
                    SpriteRenderer.flipX = !SpriteRenderer.flipX;

                _currentPoint = (_currentPoint < _points.Length - 1) ? ++_currentPoint : 0;
            }
        }
    }
}