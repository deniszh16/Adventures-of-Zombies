using UnityEngine;
using Cubra.Controllers;

namespace Cubra
{
    public class LanguageButton : ChangeSettingsButton
    {
        private LocalizationController _localizationController;

        protected override void Awake()
        {
            base.Awake();
            _localizationController = Camera.main.GetComponent<LocalizationController>();
            _localizationController.LanguageChanged += ChangeSprite;
        }

        private void Start()
        {
            var sprite = LocalizationController.Language == "ru-RU" ? 0 : 1;
            ChangeSprite(sprite);
        }
    }
}