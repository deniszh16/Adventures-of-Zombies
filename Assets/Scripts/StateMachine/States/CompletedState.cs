using Logic.Timer;
using Logic.UsefulObjects;
using Services.Ads;
using Services.PersistentProgress;
using Services.SaveLoad;
using Services.Sound;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;

namespace StateMachine.States
{
    public class CompletedState : BaseStates
    {
        [Header("Текущий уровень")]
        [SerializeField] private int _number;
        [SerializeField] private BrainsAtLevel _brainsAtLevel;
        [SerializeField] private Timer _timer;

        [Header("Панель выигрыша")]
        [SerializeField] private GameObject _victoryPanel;
        
        [Header("Звезды за уровень")]
        [SerializeField] private Image _levelStars;
        [SerializeField] private Sprite[] _spritesStars;

        [Header("Победный текст")]
        [SerializeField] private TextMeshProUGUI _textComponent;
        [SerializeField] private string[] _winningTexts;
        
        [Header("Победный звук")]
        [SerializeField] private PlayingSound _playingSound;

        private const int BonusCoefficient = 55;
        
        private IPersistentProgressService _progressService;
        private ISaveLoadService _saveLoadService;
        private ISoundService _soundService;
        private IAdService _adService;

        [Inject]
        private void Construct(IPersistentProgressService progressService, ISaveLoadService saveLoadService,
            ISoundService soundService, IAdService adService)
        {
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _soundService = soundService;
            _adService = adService;
        }
        
        public override void Enter()
        {
            _victoryPanel.SetActive(true);
            _soundService.StopBackgroundMusic();
            _playingSound.PlaySound();
            _levelStars.sprite = _spritesStars[_timer.Stars - 1];
            _textComponent.text = _winningTexts[_timer.Stars - 1];
            
            UpdateProgress();
            
            if (_number % 2 == 0)
                _adService.ShowInterstitialAd();
        }

        public override void Exit() =>
            _victoryPanel.SetActive(false);

        private void UpdateProgress()
        {
            if (_progressService.UserProgress.Progress <= _number)
                _progressService.UserProgress.Progress += 1;

            UpdateStars();
            
            _progressService.UserProgress.Brains += BrainsAtLevel.InitialValue - _brainsAtLevel.Brains;
            _progressService.UserProgress.Played += 1;
            _saveLoadService.SaveProgress();
        }

        private void UpdateStars()
        {
            if (_progressService.UserProgress.Stars[_number - 1] < _timer.Stars)
                _progressService.UserProgress.Stars[_number - 1] = _timer.Stars;
        }
    }
}