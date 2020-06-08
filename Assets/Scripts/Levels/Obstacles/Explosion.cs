using UnityEngine;

namespace Cubra
{
    public class Explosion : SharpObstacles
    {
        // Ссылки на компоненты
        private PlayingSound _playingSound;
        private CameraShaking _cameraShaking;

        protected override void Awake()
        {
            base.Awake();

            _playingSound = GetComponent<PlayingSound>();
            _cameraShaking = Camera.main.GetComponent<CameraShaking>();
        }

        /// <summary>
        /// Запуск эффектов при взрыве
        /// </summary>
        public void RunEffects()
        {
            // Воспроизводим звук
            _playingSound.PlaySound();
            // Запускаем дрожание камеры
            _ = StartCoroutine(_cameraShaking.ShakeCamera(1f, 2f, 1.7f));
        }
    }
}
