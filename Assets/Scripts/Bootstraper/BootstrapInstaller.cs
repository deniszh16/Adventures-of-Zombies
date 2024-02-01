using PimDeWitte.UnityMainThreadDispatcher;
using Services.PersistentProgress;
using Services.Achievements;
using Services.Localization;
using Services.SceneLoader;
using Services.GooglePlay;
using Services.SaveLoad;
using Services.Sound;
using Services.Ads;
using UnityEngine;
using Zenject;

namespace Bootstraper
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private SceneLoaderService _sceneLoader;
        [SerializeField] private LocalizationService _localizationService;
        [SerializeField] private SoundService _soundService;
        [SerializeField] private AchievementsService _achievementsService;
        [SerializeField] private UnityMainThreadDispatcher _mainThreadDispatcher;
        
        private IPersistentProgressService _progressService;
        private ISaveLoadService _saveLoadService;
        
        public override void InstallBindings()
        {
            BindPersistentProgress();
            BindSaveLoadService();
            BindLocalizationService();
            BindSceneLoader();
            BindSoundService();
            BindGooglePlayService();
            BindAchievementsService();
            BindAdsService();
            BindMainThreadDispatcher();
        }

        private void BindPersistentProgress()
        {
            _progressService = new PersistentProgressService();
            Container.BindInstance(_progressService).AsSingle();
        }

        private void BindSaveLoadService()
        {
            _saveLoadService = new SaveLoadService(_progressService);
            Container.BindInstance(_saveLoadService).AsSingle();
        }

        private void BindLocalizationService()
        {
            LocalizationService localizationService = Container.InstantiatePrefabForComponent<LocalizationService>(_localizationService);
            Container.Bind<ILocalizationService>().To<LocalizationService>().FromInstance(localizationService).AsSingle();
            localizationService.LoadTranslations();
        }

        private void BindSceneLoader()
        {
            SceneLoaderService sceneLoader = Container.InstantiatePrefabForComponent<SceneLoaderService>(_sceneLoader);
            Container.Bind<ISceneLoaderService>().To<SceneLoaderService>().FromInstance(sceneLoader).AsSingle();
        }

        private void BindSoundService()
        {
            SoundService soundService = Container.InstantiatePrefabForComponent<SoundService>(_soundService);
            Container.Bind<ISoundService>().To<SoundService>().FromInstance(soundService).AsSingle();
        }

        private void BindGooglePlayService()
        {
            IGooglePlayService googlePlayService = new GooglePlayService();
            googlePlayService.ActivateService();
            Container.BindInstance(googlePlayService).AsSingle();
        }
        
        private void BindAchievementsService()
        {
            AchievementsService achievementsService = Container.InstantiatePrefabForComponent<AchievementsService>(_achievementsService);
            Container.Bind<IAchievementsService>().To<AchievementsService>().FromInstance(achievementsService).AsSingle();
        }

        private void BindMainThreadDispatcher()
        {
            UnityMainThreadDispatcher mainThreadDispatcher =
                Container.InstantiatePrefabForComponent<UnityMainThreadDispatcher>(_mainThreadDispatcher);
            Container.BindInstance(mainThreadDispatcher).AsSingle();
        }

        private void BindAdsService()
        {
            IAdService adService = new AdService();
            Container.BindInstance(adService).AsSingle();
            adService.Initialization();
        }
    }
}