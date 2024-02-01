using PimDeWitte.UnityMainThreadDispatcher;
using AppodealStack.Monetization.Common;
using Services.PersistentProgress;
using Services.SaveLoad;
using Services.Ads;
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
        private void Construct(IPersistentProgressService progressService, ISaveLoadService saveLoadService,
            IAdService adService)
        {
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _adService = adService;
        }

        private void Awake()
        {
            _button.onClick.AddListener(ShowAd);
            AppodealCallbacks.RewardedVideo.OnFinished += OnRewardedVideoFinished;
        }

        private void ShowAd() =>
            _adService.ShowRewardedAd();
        
        private void OnRewardedVideoFinished(object sender, RewardedVideoFinishedEventArgs e)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(()=> {
                _progressService.GetUserProgress.ChangeBones(100);
                _saveLoadService.SaveProgress();
            });
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(ShowAd);
            AppodealCallbacks.RewardedVideo.OnFinished -= OnRewardedVideoFinished;
        }
    }
}