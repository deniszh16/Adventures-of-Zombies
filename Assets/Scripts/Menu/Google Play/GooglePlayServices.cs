using UnityEngine;

namespace Cubra
{
    public class GooglePlayServices : MonoBehaviour
    {
        [Header("Автоматическое подключение")]
        [SerializeField] private bool _autoConnect;

        // Аутентификация игрока
        public static bool Authenticated => Social.localUser.authenticated;

        private void Start()
        {
            if (_autoConnect) SignGooglePlay();
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
    }
}