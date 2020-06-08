using UnityEngine;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

namespace Cubra
{
    public class Resurrect : MonoBehaviour, IRewardedVideoAdListener
    {
        private void Start()
        {
            // Активация обратных вызовов для видеорекламы
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
                // Вычитаем стоимость воскрешения
                PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - 50);
                // Запускаем продолжение уровня
                Main.Instance.LevelResults.ResumeLevel();
            }
            else
            {
                // Иначе проверяем предзагрузку видеорекламы
                _ = Appodeal.isLoaded(Appodeal.REWARDED_VIDEO);
                // Затем отображаем рекламу
                _ = Appodeal.show(Appodeal.REWARDED_VIDEO);
            }

            // Если доступен интернет
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                // Разблокируем достижение (только не сначала)
                GooglePlayServices.UnlockingAchievement(GPGSIds.achievement_10);
            }
        }

        /// <summary>
        /// Возобновление уровня после просмотра рекламы
        /// </summary>
        public void onRewardedVideoFinished(double amount, string name)
        {
            // Запускаем продолжение уровня
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