using UnityEngine;

namespace Cubra
{
    public class Saw : SharpObstacles
    {
        [Header("Скорость движения")]
        [SerializeField] private float _speed;

        [Header("Скорость вращения")]
        [SerializeField] protected float _rotate;
        
        private Vector3 _direction;

        private void Start()
        {
            _direction = Vector3.right;
        }

        private void Update()
        {
            if (GameManager.Instance.CurrentMode == GameManager.GameModes.Play)
            {
                Transform.Rotate(Vector3.forward * _rotate * Time.deltaTime);
                Transform.localPosition = Vector3.MoveTowards(Transform.localPosition, Transform.localPosition + _direction, _speed * Time.deltaTime);

                // Если пила достигает предельной точки, меняем направление
                if (Transform.localPosition.x <= -1.7f) _direction = Vector3.left;
                else if (Transform.localPosition.x >= 1.7f) _direction = Vector3.right;
            }
        }
    }
}