using System;

namespace Services.Ads
{
    public interface IAdService
    {
        public event Action RewardedVideoFinished;
        public void ShowInterstitialAd();
        public void ShowRewardedAd();
    }
}