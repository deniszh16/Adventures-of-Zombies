using Services.Localization;
using UnityEngine;
using Zenject;
using TMPro;

namespace Logic.UI
{
    public class TextTranslation : MonoBehaviour
    {
        [Header("Запрет автоперевода")]
        [SerializeField] private bool _disableAutoTranslation;
        
        [Header("Ключ перевода")]
        [SerializeField] private string _key;
        
        [Header("Текстовый компонент")]
        [SerializeField] private TextMeshProUGUI _textComponent;

        private const string RootElement = "languages"; 
        
        private ILocalizationService _localizationService;

        [Inject]
        private void Construct(ILocalizationService localizationService) => 
            _localizationService = localizationService; 
        
        private void Awake() => 
            _localizationService.LanguageChanged += TranslateText;

        private void Start()
        {
            if (_disableAutoTranslation != true)
                TranslateText();
        }

        private void TranslateText()
        {
            string currentLanguage = _localizationService.CurrentLanguage.ToString();
            
            _textComponent.text = _localizationService.Translations?
                .Element(RootElement)?.Element(currentLanguage)?.Element(_key)?.Value;
        }
        
        public void ChangeKey(string value)
        {
            _key = value;
            TranslateText();
        } 
        
        private void OnDestroy() => 
            _localizationService.LanguageChanged -= TranslateText;
    }
}