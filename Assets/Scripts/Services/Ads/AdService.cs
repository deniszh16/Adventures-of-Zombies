using AppodealStack.Monetization.Api;
using AppodealStack.Monetization.Common;

namespace Services.Ads
{
    public class AdService : IAdService
    {
        private const string AppKey = "413a2fb3ba44d81e889d22e3f7fccbfbef2728e9ae2b50b7";

        public void Initialization()
        {
            Appodeal.MuteVideosIfCallsMuted(true);
            int adTypes = AppodealAdType.Interstitial | AppodealAdType.RewardedVideo;
            AppodealCallbacks.Sdk.OnInitialized += OnInitializationFinished;
            Appodeal.Initialize(AppKey, adTypes);
        }

        private void OnInitializationFinished(object sender, SdkInitializedEventArgs e)
        {
        }

        public void ShowInterstitialAd()
        {
            if (Appodeal.IsLoaded(AppodealAdType.Interstitial))
                Appodeal.Show(AppodealShowStyle.Interstitial);
        }

        public void ShowRewardedAd()
        {
            if (Appodeal.IsLoaded(AppodealAdType.RewardedVideo))
                Appodeal.Show(AppodealShowStyle.RewardedVideo);
        }
    }
}