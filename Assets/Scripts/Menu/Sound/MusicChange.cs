using UnityEngine;
using Cubra.Controllers;

namespace Cubra
{
    public class MusicChange : MonoBehaviour
    {
        private BackgroundMusic _backgroundMusic;

        private void Awake()
        {
            _backgroundMusic = FindObjectOfType<BackgroundMusic>();
        }

        private void Start()
        {
            if (SoundController.PlayingSounds && ChoiceSets.AtLevel)
            {
                ChoiceSets.AtLevel = false;
                // Устанавливаем фоновую музыку и запускаем
                _backgroundMusic.SetBackgroundMusic();
                _backgroundMusic.SwitchMusic((int)BackgroundMusic.State.On);
            }
        }
    }
}