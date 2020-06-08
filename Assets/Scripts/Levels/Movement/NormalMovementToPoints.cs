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
                // Перемещаем объект в указанном направлении
                Transform.position = Vector3.MoveTowards(Transform.position, _points[_currentPoint], _speed * Time.deltaTime);
            }
            else
            {
                if (_turningSprite)
                {
                    // Горизонтально разворачиваем спрайт
                    SpriteRenderer.flipX = !SpriteRenderer.flipX;
                }

                // Переключаемся на следующую точку, либо сбрасываем ее на начальную
                _currentPoint = (_currentPoint < _points.Length - 1) ? ++_currentPoint : 0;
            }
        }
    }
}