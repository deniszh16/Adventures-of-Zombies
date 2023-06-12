using System;
using System.Collections;
using Services.PersistentProgress;
using Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace Services.Sound
{
    public class SoundService : MonoBehaviour, ISoundService
    {
        [Header("Музыка меню")]
        [SerializeField] private AudioClip[] _musicOnMenu;
        
        [Header("Музыка уровней")]
        [SerializeField] private AudioClip[] _musicOnLevels;
        
        [Header("Музыкальный компонент")]
        [SerializeField] private AudioSource _audioSource;
        
        public bool SoundActivity { get; set; }
        
        public event Action SoundChanged;

        private IPersistentProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        [Inject]
        private void Construct(IPersistentProgressService progressService, ISaveLoadService saveLoadService)
        {
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }

        public void SwitchSound()
        {
            bool activity = _progressService.UserProgress.SoundData.Activity;
            
            SoundActivity = !activity;
            _progressService.UserProgress.SoundData.SetSoundActivity(SoundActivity);
            SoundChanged?.Invoke();

            ChangeVolume();
            _saveLoadService.SaveProgress();
        }

        public void StartBackgroundMusicInMenu()
        {
            if (_musicOnMenu.Length > 0)
            {
                int number = UnityEngine.Random.Range(0, _musicOnMenu.Length);
                _audioSource.clip = _musicOnMenu[number];
                
                if (SoundActivity && _audioSource.isPlaying != true)
                    _audioSource.Play();
            }
        }

        public void PrepareBackgroundMusicOnLevel()
        {
            if (_musicOnLevels.Length > 0)
            {
                int number = UnityEngine.Random.Range(0, _musicOnLevels.Length);
                _audioSource.clip = _musicOnLevels[number];
            }
        }

        public void StartBackgroundMusicOnLevels()
        {
            if (SoundActivity && _audioSource.isPlaying != true)
                _audioSource.Play();
        }

        public void ChangeVolume()
        {
            if (SoundActivity)
                StartCoroutine(ChangeVolumeCoroutine(
                    valueComparison: () => _audioSource.volume < 0.2f, delay: 0.02f, changeStep: 0.01f));
            else
                StartCoroutine(ChangeVolumeCoroutine(
                    valueComparison: () => _audioSource.volume < 0f, delay: 0.02f, changeStep: -0.03f));
        }

        private IEnumerator ChangeVolumeCoroutine(Func<bool> valueComparison, float delay, float changeStep)
        {
            if (changeStep > 0) _audioSource.Play();

            while (valueComparison())
            {
                yield return new WaitForSeconds(delay);
                _audioSource.volume += changeStep;
            }
            
            if (changeStep < 0) _audioSource.Stop();
        }

        public void PauseBackgroundMusic(bool pause)
        {
            if (pause) _audioSource.Pause();
            else _audioSource.Play();
        }

        public void StopBackgroundMusic() =>
            _audioSource.Stop();
    }
}