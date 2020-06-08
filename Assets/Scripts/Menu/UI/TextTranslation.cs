using UnityEngine;
using UnityEngine.UI;
using Cubra.Controllers;
using Cubra.Helpers;

namespace Cubra
{
    public class TextTranslation : MonoBehaviour
    {
        [Header("Автоперевод текста")]
        [SerializeField] private bool _autoTranslation = true;

        [Header("Ключ перевода")]
        [SerializeField] private string _key;

        // Ссылка на компонент
        private Text _textComponent;

        private void Awake()
        {
            _textComponent = GetComponent<Text>();
        }

        private void Start()
        {
            if (_autoTranslation) TranslateText();
        }

        /// <summary>
        /// Обновление перевода в текстовом поле
        /// </summary>
        public void TranslateText()
        {
            // Получаем текущий язык игры
            var currentLanguage = LocalizationController.Language;

            // Устанавливаем текст с указанным ключом из xml файла
            _textComponent.text = FileParseHelper.Languages.Element("languages").Element(currentLanguage).Element(_key).Value;
        }

        /// <summary>
        /// Изменение ключа перевода
        /// </summary>
        /// <param name="value">новый ключ</param>
        public void ChangeKey(string value)
        {
            _key = value;
            TranslateText();
        }
    }
}