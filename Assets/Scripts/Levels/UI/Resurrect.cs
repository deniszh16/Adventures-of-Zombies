using UnityEngine;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

namespace Cubra
{
    public class Resurrect : MonoBehaviour, IRewardedVideoAdListener
    {
        private void Start()
        {
            Appodeal.setRewardedVideoCallbacks(this);
        }

        /// <summary>
        /// Воскрешение персонажа
        /// </summary>
        public void ResurrectCharacter()
        {
            // Если достаточно монет
            if (PlayerPrefs.GetInt("coins") >= 50)
            {
                PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - 50);
                Main.Instance.LevelResults.ResumeLevel();
            }
            else
            {
                // Иначе вызываем видеорекламу
                Appodeal.isLoaded(Appodeal.REWARDED_VIDEO);
                Appodeal.show(Appodeal.REWARDED_VIDEO);
            }

            if (Application.internetReachability != NetworkReachability.NotReachable)
                GooglePlayServices.UnlockingAchievement(GPGSIds.achievement_10);
        }

        /// <summary>
        /// Возобновление уровня после просмотра рекламы
        /// </summary>
        public void onRewardedVideoFinished(double amount, string name)
        {
            Main.Instance.LevelResults.ResumeLevel();
        }

        public void onRewardedVideoLoaded(bool isPrecache) {}
        public void onRewardedVideoFailedToLoad() {}
        public void onRewardedVideoShown() {}
        public void onRewardedVideoClosed(bool finished) {}
        public void onRewardedVideoClicked() {}
        public void onRewardedVideoExpired() {}
        public void onRewardedVideoShowFailed() {}
    }
}