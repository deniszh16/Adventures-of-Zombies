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

        // Номер текущего объекта
        public int _objectNumber;

        // Ссылка на контроллер звука
        private SoundController _soundController;

        protected override void Awake()
        {
            base.Awake();

            if (SoundController.BackgroundMusic == 0)
            {
                SoundController.BackgroundMusic = 1;
                _objectNumber = SoundController.BackgroundMusic;
                DontDestroyOnLoad(gameObject);
            }

            if (_objectNumber != 1) Destroy(gameObject);
        }

        private void Start()
        {
            SetBackgroundMusic();

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
            // Список фоновой музыки
            var audioClips = Camera.main.GetComponent<MusicListHelper>().AudioClips;

            if (audioClips.Length > 1)
            {
                _audioSource.clip = audioClips[UnityEngine.Random.Range(0, audioClips.Length)];
            }
            else
            {
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
                _ = StartCoroutine(ChangeVolume(() => _audioSource.volume < 0.2f, 0.02f, 0.01f));
            }
            else
            {
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
            if (value > 0) _audioSource.Play();

            while (func())
            {
                yield return new WaitForSeconds(pause);
                _audioSource.volume += value;
            }

            if (value < 0) _audioSource.Stop();
        }
    }
}