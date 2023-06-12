using Services.Sound;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Logic.UI
{
    public class SoundSwitchButton : MonoBehaviour
    {
        [Header("Спрайты кнопок")]
        [SerializeField] private Sprite[] _sprites;

        [Header("Кнопка переключения")]
        [SerializeField] private Button _button;
        [SerializeField] private Image _imageButton;

        private ISoundService _soundService;

        [Inject]
        private void Construct(ISoundService soundService) =>
            _soundService = soundService;

        private void Awake()
        {
            _button.onClick.AddListener(ChangeSoundSetting);
            _soundService.SoundChanged += SetButtonSprite;
        }

        private void Start() =>
            SetButtonSprite();

        private void SetButtonSprite()
        {
            int spriteNumber = _soundService.SoundActivity ? 0 : 1;
            _imageButton.sprite = _sprites[spriteNumber];
        }

        private void ChangeSoundSetting() =>
            _soundService.SwitchSound();

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(ChangeSoundSetting);
            _soundService.SoundChanged -= SetButtonSprite;
        }
    }
}