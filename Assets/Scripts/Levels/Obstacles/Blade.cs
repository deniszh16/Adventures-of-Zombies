using System.Collections;
using UnityEngine;

namespace Cubra
{
    public class Blade : SharpObstacles
    {
        // Секунды до повторения ускорения
        private float _secondsToRepeat = 3f;

        // Ссылка на физический компонент
        private Rigidbody2D _rigidbody;

        protected override void Awake()
        {
            base.Awake();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            Main.Instance.LevelLaunched += StartAcceleration;
        }

        /// <summary>
        /// Запуск раскачивания лезвия
        /// </summary>
        private void StartAcceleration()
        {
            _ = StartCoroutine(SpeedUpRocking());
        }

        /// <summary>
        /// Переодическое увеличение скорости раскачивания лезвия
        /// </summary>
        private IEnumerator SpeedUpRocking()
        {
            while (Main.Instance.CurrentMode == Main.GameModes.Play)
            {
                yield return new WaitForSeconds(_secondsToRepeat);
                // Увеличиваем физическую скорость
                _rigidbody.velocity *= 1.2f;
            }
        }
    }
}