using System.Collections;
using UnityEngine;

namespace Cubra
{
    public class RotateObject : BaseObjects
    {
        [Header("Скорость поворота")]
        [SerializeField] private float _speed;

        [Header("Пауза между поворотами")]
        [SerializeField] private float _pause;

        // Угол для поворота
        private int _angle = 0;

        private void Start()
        {
            GameManager.Instance.LevelLaunched += StartRotation;
        }
        
        private void StartRotation()
        {
            _ = StartCoroutine(ChangeAngle());
        }

        /// <summary>
        /// Увеличение целевого угла на 90 градусов
        /// </summary>
        private IEnumerator ChangeAngle()
        {
            while (GameManager.Instance.CurrentMode == GameManager.GameModes.Play)
            {
                yield return new WaitForSeconds(_pause);
                _angle += 90;
            }
        }

        private void Update()
        {
            if ((int)Transform.localEulerAngles.z == 0 && _angle >= 360) _angle = 0;
            if (Transform.localEulerAngles.z < _angle) Transform.Rotate(Vector3.forward, _speed * Time.deltaTime);
        }
    }
}