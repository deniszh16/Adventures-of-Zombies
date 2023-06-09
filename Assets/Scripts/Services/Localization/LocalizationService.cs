using System;
using System.Xml.Linq;
using Services.PersistentProgress;
using Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace Services.Localization
{
    public class LocalizationService : MonoBehaviour, ILocalizationService
    {
        public XDocument Translations { get; set; }
        
        public Languages CurrentLanguage { get; set; }
        public event Action LanguageChanged;

        private const string FileName = "languages";

        private IPersistentProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        [Inject]
        public void Construct(IPersistentProgressService progressService, ISaveLoadService saveLoadService)
        {
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }

        public void LoadTranslations() =>
            Translations = XmlFileParse.ParseXmlFromResources(FileName);

        public void SwitchLanguage()
        {
            Languages language = _progressService.UserProgress.languageData.GetCurrentLanguage();
            if (language == Languages.Russian)
            {
                _progressService.UserProgress.languageData.SetLanguage(Languages.English);
                CurrentLanguage = Languages.English;
            }
            else
            {
                _progressService.UserProgress.languageData.SetLanguage(Languages.English);
                CurrentLanguage = Languages.English;
            }
            
            LanguageChanged?.Invoke();
            _saveLoadService.SaveProgress();
        }
    }
}