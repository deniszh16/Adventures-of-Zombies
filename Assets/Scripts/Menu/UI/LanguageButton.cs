using UnityEngine;
using Cubra.Controllers;

namespace Cubra
{
    public class LanguageButton : ChangeSettingsButton
    {
        // Ссылка на компонент
        private LocalizationController _localizationController;

        protected override void Awake()
        {
            base.Awake();
            _localizationController = Camera.main.GetComponent<LocalizationController>();

            // Подписываем изменение спрайта кнопки
            _localizationController.LanguageChanged += ChangeSprite;
        }

        private void Start()
        {
            // Получаем номер спрайта в зависимости от сохраненного значения
            var sprite = LocalizationController.Language == "ru-RU" ? 0 : 1;
            // Устанавливаем спрайт
            ChangeSprite(sprite);
        }
    }
}