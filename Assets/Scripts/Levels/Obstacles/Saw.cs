using UnityEngine;

namespace Cubra
{
    public class Saw : SharpObstacles
    {
        [Header("Скорость движения")]
        [SerializeField] private float _speed;

        [Header("Скорость вращения")]
        [SerializeField] protected float _rotate;

        // Вектор движения пилы
        private Vector3 _direction;

        private void Start()
        {
            _direction = Vector3.right;
        }

        private void Update()
        {
            if (Main.Instance.CurrentMode == Main.GameModes.Play)
            {
                // Вращаем пилу
                Transform.Rotate(Vector3.forward * _rotate);

                // Двигаем пилу в указанном направлении с указанной скоростью
                Transform.localPosition = Vector3.MoveTowards(Transform.localPosition, Transform.localPosition + _direction, _speed * Time.deltaTime);

                // Если пила достигает предельной точки, меняем направление
                if (Transform.localPosition.x <= -1.7f || Transform.localPosition.x >= 1.7f)
                {
                    _direction *= -1;
                }
            }
        }
    }
}