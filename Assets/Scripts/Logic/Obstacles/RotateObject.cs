using System.Collections;
using UnityEngine;

namespace Logic.Obstacles
{
    public class RotateObject : MonoBehaviour
    {
        [Header("Скорость поворота")]
        [SerializeField] private float _speed;

        [Header("Пауза между поворотами")]
        [SerializeField] private float _pause;

        private Transform _transform;
        private int _angle = 0;

        private void Awake() =>
            _transform = transform;

        private void Start() =>
            _ = StartCoroutine(ChangeAngle());

        private void Update()
        {
            if ((int)_transform.localEulerAngles.z == 0 && _angle >= 360) _angle = 0;
            
            if (_transform.localEulerAngles.z < _angle)
                _transform.Rotate(Vector3.forward, _speed * Time.deltaTime);
        }

        private IEnumerator ChangeAngle()
        {
            while (true)
            {
                yield return new WaitForSeconds(_pause);
                _angle += 90;
            }
        }
    }
}