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
            if (SoundController.PlayingSounds && SetsHelper.AtLevel)
            {
                SetsHelper.AtLevel = false;
                _backgroundMusic.SetBackgroundMusic();
                _backgroundMusic.SwitchMusic((int)BackgroundMusic.State.On);
            }
        }
    }
}