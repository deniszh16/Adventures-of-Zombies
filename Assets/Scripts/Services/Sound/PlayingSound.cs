﻿using UnityEngine;
using Zenject;

namespace Services.Sound
{
    public class PlayingSound : MonoBehaviour
    {
        [Header("Компонент звука")]
        [SerializeField] private AudioSource _audioSource;

        private ISoundService _soundService;

        [Inject]
        private void Construct(ISoundService soundService) =>
            _soundService = soundService;

        public void PlaySound()
        {
            if (_soundService.SoundActivity)
                _audioSource.Play();
        }
    }
}