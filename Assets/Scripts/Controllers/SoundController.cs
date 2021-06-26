using System;
using UnityEngine;

namespace Cubra.Controllers
{
    public class SoundController : BaseController
    {
        // Событие по переключению звука
        public event Action<int> SoundChanged;

        // Номер объекта фоновой музыки
        public static int BackgroundMusic = 0;

        private void Start()
        {
            var backgroundMusic = FindObjectOfType<BackgroundMusic>();
            SoundChanged += backgroundMusic.SwitchMusic;
        }

        // Текущее состояние звука
        public static int SoundActivity
        {
            get => PlayerPrefs.GetInt("sounds");
            set => PlayerPrefs.SetInt("sounds", value);
        }

        // Активность проигрывания звуков
        public static bool PlayingSounds
        {
            get => SoundActivity == 0;
        }    

        /// <summary>
        /// Переключение звука
        /// </summary>
        public void SwitchSound()
        {
            SoundActivity = SoundActivity > 0 ? 0 : 1;
            SoundChanged?.Invoke(SoundActivity);
        }
    }
}