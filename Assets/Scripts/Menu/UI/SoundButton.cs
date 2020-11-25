using UnityEngine;
using Cubra.Controllers;

namespace Cubra
{
    public class SoundButton : ChangeSettingsButton
    {
        private SoundController _soundController;

        protected override void Awake()
        {
            base.Awake();
            _soundController = Camera.main.GetComponent<SoundController>();

            // Подписываем изменение спрайта кнопки
            _soundController.SoundChanged += ChangeSprite;
        }

        private void Start()
        {
            // Устанавливаем спрайт кнопки при старте
            ChangeSprite(SoundController.SoundActivity);
        }
    }
}