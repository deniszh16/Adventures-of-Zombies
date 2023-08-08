﻿using Services.PersistentProgress;
using Services.Localization;
using Services.SceneLoader;
using Services.SaveLoad;
using Services.Sound;
using UnityEngine;
using Zenject;
using Data;

namespace Bootstraper
{
    public class GameBootstraper : MonoBehaviour
    {
        private ISceneLoaderService _sceneLoaderService;
        private IPersistentProgressService _progressService;
        private ILocalizationService _localizationService;
        private ISaveLoadService _saveLoadService;
        private ISoundService _soundService;

        [Inject]
        private void Construct(ISceneLoaderService sceneLoaderService, IPersistentProgressService progressService,
            ILocalizationService localizationService, ISaveLoadService saveLoadService, ISoundService soundService)
        {
            _sceneLoaderService = sceneLoaderService;
            _progressService = progressService;
            _localizationService = localizationService;
            _saveLoadService = saveLoadService;
            _soundService = soundService;
        }

        private void Awake() =>
            Application.targetFrameRate = 60;

        private void Start()
        {
            LoadProgressOrInitNew();
            _soundService.SoundActivity = _progressService.UserProgress.SoundData.Activity;
            _localizationService.SetCurrentLanguage(_progressService.UserProgress.LanguageData.Language);
            _sceneLoaderService.LoadSceneAsync(Scenes.MainMenu, screensaver: false, delay: 1.5f);
        }

        private void LoadProgressOrInitNew()
        {
            _progressService.UserProgress =
                _saveLoadService.LoadProgress() ?? new UserProgress();
        }
    }
}