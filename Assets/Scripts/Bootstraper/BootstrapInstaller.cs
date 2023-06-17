using Services.Achievements;
using Services.Localization;
using Services.PersistentProgress;
using Services.SaveLoad;
using Services.SceneLoader;
using Services.Sound;
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
        
        private IPersistentProgressService _progressService;
        private ISaveLoadService _saveLoadService;
        
        public override void InstallBindings()
        {
            BindPersistentProgress();
            BindSaveLoadService();
            BindLocalizationService();
            BindSceneLoader();
            BindSoundService();
            BindAchievementsService();
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
        
        private void BindAchievementsService()
        {
            AchievementsService achievementsService = Container.InstantiatePrefabForComponent<AchievementsService>(_achievementsService);
            Container.Bind<IAchievementsService>().To<AchievementsService>().FromInstance(achievementsService).AsSingle();
        }
    }
}