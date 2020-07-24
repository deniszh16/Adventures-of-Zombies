using UnityEngine;

namespace Cubra
{
    public class GooglePlayServices : MonoBehaviour
    {
        // Аутентификация игрока
        public static bool Authenticated => Social.localUser.authenticated;

        private void Start()
        {
            SignGooglePlay();
        }

        /// <summary>
        /// Подключение к сервисам Google Play
        /// </summary>
        private static void SignGooglePlay()
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                Social.localUser.Authenticate((bool success) => {});
            }  
        }

        /// <summary>
        /// Просмотр игровых достижений
        /// </summary>
        public void ShowAchievements()
        {
            if (Authenticated)
            {
                Social.ShowAchievementsUI();
            }
            else
            {
                SignGooglePlay();
            }
        }

        /// <summary>
        /// Разблокировка нового достижения
        /// </summary>
        /// <param name="identifier">идентификатор достижения</param>
        public static void UnlockingAchievement(string identifier)
        {
            if (Authenticated) Social.ReportProgress(identifier, 100.0f, (bool success) => {});
        }

        /// <summary>
        /// Отправка результата в таблицу лидеров
        /// </summary>
        /// <param name="score">игровой счет</param>
        public static void PostingScoreLeaderboard(int score)
        {
            if (Authenticated) Social.ReportScore(score, GPGSIds.leaderboard, (bool success) => {});
        }
    }
}