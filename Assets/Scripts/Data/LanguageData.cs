using System;
using Services.Localization;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class LanguageData
    {
        private Languages _language;

        public LanguageData() =>
            _language = Application.systemLanguage == SystemLanguage.Russian ? Languages.Russian : Languages.English;

        public Languages GetCurrentLanguage() =>
            _language;

        public void SetLanguage(Languages language) =>
            _language = language;
    }
}