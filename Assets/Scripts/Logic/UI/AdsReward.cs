using AppodealStack.Monetization.Common;
using PimDeWitte.UnityMainThreadDispatcher;
using Services.Ads;
using Services.PersistentProgress;
using Services.SaveLoad;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Logic.UI
{
    public class AdsReward : MonoBehaviour
    {
        [Header("Кнопка бонуса")]
        [SerializeField] private Button _button;

        private IPersistentProgressService _progressService;
        private ISaveLoadService _saveLoadService;
        private IAdService _adService;

        [Inject]
        private void Construct(IPersistentProgressService progressService, ISaveLoadService saveLoadService, IAdService adService)
        {
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _adService = adService;
        }

        private void Awake()
        {
            _button.onClick.AddListener(ShowAd);
            AppodealCallbacks.RewardedVideo.OnClosed += OnRewardedVideoClosed;
        }

        private void ShowAd() =>
            _adService.ShowRewardedAd();
        
        private void OnRewardedVideoClosed(object sender, RewardedVideoClosedEventArgs e)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(()=> {
                _progressService.GetUserProgress.ChangeBones(100);
                _saveLoadService.SaveProgress();
            });
        }

        private void OnDestroy() =>
            _button.onClick.RemoveListener(ShowAd);
    }
}