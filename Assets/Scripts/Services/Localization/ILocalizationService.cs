using System;
using System.Xml.Linq;

namespace Services.Localization
{
    public interface ILocalizationService
    {
        public Languages CurrentLanguage { get; set; }
        public XDocument Translations { get; set; }
        
        public event Action LanguageChanged;
        
        public void LoadTranslations();
        public void SwitchLanguage();
    }
}