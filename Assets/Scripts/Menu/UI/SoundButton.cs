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
            _soundController.SoundChanged += ChangeSprite;
        }

        private void Start()
        {
            ChangeSprite(SoundController.SoundActivity);
        }
    }
}