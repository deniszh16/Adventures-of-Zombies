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
            var currentLanguage = LocalizationController.Language;
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