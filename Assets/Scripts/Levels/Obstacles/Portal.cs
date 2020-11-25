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
            GameManager.Instance.LevelLaunched += LaunchTeleportation;
        }
        
        private void LaunchTeleportation()
        {
            _ = StartCoroutine(TeleportPlatform());
        }

        /// <summary>
        /// Переставление платформы в активную точку
        /// </summary>
        private IEnumerator TeleportPlatform()
        {
            while (GameManager.Instance.CurrentMode == GameManager.GameModes.Play)
            {
                yield return new WaitForSeconds(_pause);

                _point++;

                if (_point > _points.Length - 1) _point = 0;

                Transform.position = _points[_point];
            }
        }
    }
}