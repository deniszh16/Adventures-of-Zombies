using UnityEngine;
using GooglePlayGames;

public class PlayServices : MonoBehaviour
{
    private void Start()
    {
        // Если интернет доступен, подключаемся к Google Play
        if (Application.internetReachability != NetworkReachability.NotReachable) SignGooglePlay();
    }

    /// <summary>Подключение к сервисам Google Play</summary>
    private static void SignGooglePlay()
    {
        Social.localUser.Authenticate((bool success) => {});
    }

    /// <summary>Просмотр игровых достижений</summary>
    public static void ShowAchievements()
    {
        // Если пользователь вошел в аккаунт, отображаем список достижений
        if (Social.localUser.authenticated) Social.ShowAchievementsUI();
        // Иначе подключаемся к Google Play
        else SignGooglePlay();
    }

    /// <summary>Разблокировка нового достижения (идентификатор достижения)</summary>
    public static void UnlockingAchievement(string identifier)
    {
        if (Social.localUser.authenticated)
            Social.ReportProgress(identifier, 100.0f, (bool success) => {});
    }

    /// <summary>Просмотр таблицы лидеров</summary>
    public static void ShowLeaderboard()
    {
        //Если пользователь вошел в аккаунт, отображаем таблицу игроков
        if (Social.localUser.authenticated) PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard);
        // Иначе подключаемся к Google Play
        else SignGooglePlay();
    }

    /// <summary>Отправка результата в таблицу лидеров</summary>
    public static void PostingScoreLeaderboard(int score)
    {
        if (Social.localUser.authenticated)
            Social.ReportScore(score, GPGSIds.leaderboard, (bool success) => {});
    }
}