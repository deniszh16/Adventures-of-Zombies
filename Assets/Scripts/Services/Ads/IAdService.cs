﻿namespace Services.Ads
{
    public interface IAdService
    {
        public void Initialization();
        public void ShowInterstitialAd();
        public void ShowRewardedAd();
    }
}