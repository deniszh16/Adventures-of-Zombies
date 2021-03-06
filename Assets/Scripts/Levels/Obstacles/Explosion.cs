﻿using UnityEngine;

namespace Cubra
{
    public class Explosion : SharpObstacles
    {
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
            _playingSound.PlaySound();
            _ = StartCoroutine(_cameraShaking.ShakeCamera(1f, 2f, 1.7f));
        }
    }
}