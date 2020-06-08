using System;
using System.Collections;
using UnityEngine;
using Cubra.Controllers;
using Cubra.Helpers;

namespace Cubra
{
    public class BackgroundMusic : Sound
    {
        // Состояние звука
        public enum State { On, Off }

        // Ссылка на контроллер звука
        private SoundController _soundController;

        protected override void Awake()
        {
            base.Awake();
            _soundController = Camera.main.GetComponent<SoundController>();

            // Подписываем переключение музыки
            _soundController.SoundChanged += SwitchMusic;

            SetBackgroundMusic();
        }

        private void Start()
        {
            // Если звуки не отключены, но музыка не проигрывается
            if (SoundController.SoundActivity == (int)State.On && _audioSource.isPlaying == false)
            {
                SwitchMusic((int)State.On);
            }
        }

        /// <summary>
        /// Установка фоновой музыки
        /// </summary>
        public void SetBackgroundMusic()
        {
            // Получаем список фоновой музыки
            var audioClips = Camera.main.GetComponent<MusicListHelper>().AudioClips;

            if (audioClips.Length > 1)
            {
                // Определяем случайную фоновую музыку из доступных
                _audioSource.clip = audioClips[UnityEngine.Random.Range(0, audioClips.Length)];
            }
            else
            {
                // Иначе устанавливаем первую
                _audioSource.clip = audioClips[0];
            }
        }

        /// <summary>
        /// Включение и отключение фоновой музыки
        /// </summary>
        public void SwitchMusic(int state)
        {
            if (state == (int)State.On)
            {
                // Увеличиваем громкость фоновой музыки
                _ = StartCoroutine(ChangeVolume(() => _audioSource.volume < 0.6f, 0.02f, 0.01f));
            }
            else
            {
                // Уменьшаем громкость фоновой музыки
                _ = StartCoroutine(ChangeVolume(() => _audioSource.volume > 0, 0.02f, -0.03f));
            }
        }

        /// <summary>
        /// Изменение громкости фоновой музыки
        /// </summary>
        /// <param name="func">функция с условием</param>
        /// <param name="pause">пауза при изменении</param>
        /// <param name="value">шаг изменения</param>
        private IEnumerator ChangeVolume(Func<bool> func, float pause, float value)
        {
            // Если громкость увеличивается, запускаем музыку
            if (value > 0) _audioSource.Play();

            while (func())
            {
                yield return new WaitForSeconds(pause);
                // Изменяем громкость
                _audioSource.volume += value;
            }

            // Если громкость уменьшается, останавливаем музыку
            if (value < 0) _audioSource.Stop();
        }
    }
}