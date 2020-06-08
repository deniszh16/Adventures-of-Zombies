using System.Collections;
using UnityEngine;

namespace Cubra
{
    public class Portal : BaseObjects
    {
        [Header("Пауза для переходов")]
        [SerializeField] private float _pause;

        [Header("Точки переходов")]
        [SerializeField] private Vector3[] _points;

        // Активная точка
        private int _point = 0;

        private void Start()
        {
            Main.Instance.LevelLaunched += LaunchTeleportation;
        }

        /// <summary>
        /// Запуск телепорта платформы
        /// </summary>
        private void LaunchTeleportation()
        {
            _ = StartCoroutine(TeleportPlatform());
        }

        private IEnumerator TeleportPlatform()
        {
            while (Main.Instance.CurrentMode == Main.GameModes.Play)
            {
                yield return new WaitForSeconds(_pause);
                _point++;

                // Если точка вышла за пределы массива
                if (_point > _points.Length - 1)
                {
                    // Обнуляем точку
                    _point = 0;
                }

                // Переставляем платформу к активной точке
                Transform.position = _points[_point];
            }
        }
    }
}