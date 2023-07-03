using Services.Ads;
using UnityEngine;
using Zenject;

namespace Logic.UI
{
    public class Ads : MonoBehaviour
    {
        private IAdService _adService;

        [Inject]
        private void Construct(IAdService adService) =>
            _adService = adService;

        public void ShowAds() =>
            _adService.ShowInterstitialAd();
    }
}