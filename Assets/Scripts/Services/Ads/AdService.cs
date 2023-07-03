using System;
using System.Collections.Generic;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using UnityEngine;

namespace Services.Ads
{
    public class AdService : IAdService, IAppodealInitializationListener, IRewardedVideoAdListener
    {
        private const string AppKey = "413a2fb3ba44d81e889d22e3f7fccbfbef2728e9ae2b50b7";
        
        public event Action RewardedVideoFinished;

        public AdService()
        {
            Appodeal.setRewardedVideoCallbacks(this);
            Initialization();
        }

        private void Initialization()
        {
            Appodeal.muteVideosIfCallsMuted(true);
            int adTypes = Appodeal.INTERSTITIAL | Appodeal.REWARDED_VIDEO;
            Appodeal.initialize(AppKey, adTypes, this);
        }

        public void ShowInterstitialAd()
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                if (Appodeal.isLoaded(Appodeal.INTERSTITIAL))
                    Appodeal.show(Appodeal.INTERSTITIAL);
            }
        }

        public void ShowRewardedAd()
        {
            if (Appodeal.isLoaded(Appodeal.REWARDED_VIDEO))
                Appodeal.show(Appodeal.REWARDED_VIDEO);
        }
        
        public void onRewardedVideoFinished(double amount, string name) =>
            RewardedVideoFinished?.Invoke();

        #region Appodeal (other methods)
        public void onInitializationFinished(List<string> errors)
        {
        }

        public void onRewardedVideoLoaded(bool precache)
        {
        }

        public void onRewardedVideoFailedToLoad()
        {
        }

        public void onRewardedVideoShowFailed()
        {
        }

        public void onRewardedVideoShown()
        {
        }

        public void onRewardedVideoClosed(bool finished)
        {
        }

        public void onRewardedVideoExpired()
        {
        }

        public void onRewardedVideoClicked()
        {
        }
        #endregion
    }
}