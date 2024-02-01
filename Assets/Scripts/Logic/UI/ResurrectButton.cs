using PimDeWitte.UnityMainThreadDispatcher;
using AppodealStack.Monetization.Common;
using StateMachine.States;
using Logic.Characters;
using Services.Ads;
using StateMachine;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Logic.UI
{
    public class ResurrectButton : MonoBehaviour
    {
        [Header("Кнопка воскрешения")]
        [SerializeField] private Button _button;

        [Header("Персонаж")]
        [SerializeField] private Character _character;
        
        [Header("Компонент таймера")]
        [SerializeField] private Timer.Timer _timer;

        private GameStateMachine _gameStateMachine;
        private IAdService _adService;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine, IAdService adService)
        {
            _gameStateMachine = gameStateMachine;
            _adService = adService;
        }

        private void Awake()
        {
            _button.onClick.AddListener(ShowAd);
            AppodealCallbacks.RewardedVideo.OnFinished += OnRewardedVideoFinished;
        }
        
        private void ShowAd() =>
            _adService.ShowRewardedAd();
        
        private void OnRewardedVideoFinished(object sender, RewardedVideoFinishedEventArgs e) =>
            UnityMainThreadDispatcher.Instance().Enqueue(ResurrectACharacter);

        private void ResurrectACharacter()
        {
            _character.CharacterRespawn();
            if (_timer.Seconds < 30) _timer.Seconds = 30;
            _gameStateMachine.Enter<PlayState>();
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(ShowAd);
            AppodealCallbacks.RewardedVideo.OnFinished -= OnRewardedVideoFinished;
        }
    }
}