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
            Main.Instance.LevelLaunched += StartRotation;
        }

        /// <summary>
        /// Запуск вращения объекта
        /// </summary>
        private void StartRotation()
        {
            _ = StartCoroutine(ChangeAngle());
        }

        /// <summary>
        /// Увеличение целевого угла на 90 градусов
        /// </summary>
        private IEnumerator ChangeAngle()
        {
            while (Main.Instance.CurrentMode == Main.GameModes.Play)
            {
                yield return new WaitForSeconds(_pause);
                _angle += 90;
            }
        }

        private void Update()
        {
            // Если текущий угол нулевой, а переменная набрала полный оборот
            if ((int)Transform.localEulerAngles.z == 0 && _angle >= 360)
            {
                // Обнуляем угол
                _angle = 0;
            }

            // Если текущий угол меньше целевого угла
            if (Transform.localEulerAngles.z < _angle)
            {
                // Выполняем поворот объекта
                Transform.Rotate(Vector3.forward, _speed);
            }
        }
    }
}