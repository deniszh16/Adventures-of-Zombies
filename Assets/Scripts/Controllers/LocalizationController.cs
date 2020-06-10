using System;
using UnityEngine;
using Cubra.Helpers;

namespace Cubra.Controllers
{
    public class LocalizationController : BaseController
    {
        // Событие по переключению языка
        public event Action<int> LanguageChanged;

        // Текущий язык интерфейса
        public static string Language
        {
            get => PlayerPrefs.GetString("current-language");
            set => PlayerPrefs.SetString("current-language", value);
        }

        // Перечисления языков игры
        private enum Languages { Russian, English }

        private void Awake()
        {
            FileParseHelper.ParseXml("languages");
        }

        /// <summary>
        /// Переключение языка игры
        /// </summary>
        public void SwitchLanguage()
        {
            Language = (Language == "ru-RU") ? "en-US" : "ru-RU";

            // Номер языка в перечислении
            var language = (Language == "ru-RU") ? (int)Languages.Russian : (int)Languages.English;
            LanguageChanged?.Invoke(language);
        }
    }
}