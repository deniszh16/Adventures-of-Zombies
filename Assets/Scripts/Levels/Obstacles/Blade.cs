using System.Collections;
using UnityEngine;

namespace Cubra
{
    public class Blade : SharpObstacles
    {
        // Секунды до повторения ускорения
        private float _secondsToRepeat = 2.6f;

        private Rigidbody2D _rigidbody;

        protected override void Awake()
        {
            base.Awake();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            GameManager.Instance.LevelLaunched += StartAcceleration;
        }
        
        private void StartAcceleration()
        {
            _ = StartCoroutine(SpeedUpRocking());
        }

        /// <summary>
        /// Переодическое увеличение скорости раскачивания лезвия
        /// </summary>
        private IEnumerator SpeedUpRocking()
        {
            var seconds = new WaitForSeconds(_secondsToRepeat);

            while (GameManager.Instance.CurrentMode == GameManager.GameModes.Play)
            {
                yield return seconds;
                _rigidbody.velocity *= 1.2f;
            }
        }
    }
}