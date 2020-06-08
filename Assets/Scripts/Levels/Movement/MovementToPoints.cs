using UnityEngine;

namespace Cubra
{
    public abstract class MovementToPoints : BaseObjects
    {
        [Header("Точки для перемещения")]
        [SerializeField] protected Vector3[] _points;

        // Активная точка
        protected int _currentPoint = 0;

        [Header("Скорость движения")]
        [SerializeField] protected float _speed;

        [Header("Разворот спрайта")]
        [SerializeField] protected bool _turningSprite;
    }
}