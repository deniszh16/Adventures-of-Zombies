using AppodealStack.Monetization.Common;
using PimDeWitte.UnityMainThreadDispatcher;
using Logic.Characters;
using Services.Ads;
using StateMachine;
using StateMachine.States;
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
            AppodealCallbacks.RewardedVideo.OnClosed += OnRewardedVideoClosed;
        }
        
        private void ShowAd() =>
            _adService.ShowRewardedAd();
        
        private void OnRewardedVideoClosed(object sender, RewardedVideoClosedEventArgs e) =>
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
            AppodealCallbacks.RewardedVideo.OnClosed -= OnRewardedVideoClosed;
        }
    }
}