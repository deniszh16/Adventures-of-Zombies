using UnityEngine;
using GooglePlayGames;

public class PlayServices : MonoBehaviour
{
    private void Start()
    {
        // Если интернет доступен, подключаемся к Google Play
        if (Application.internetReachability != NetworkReachability.NotReachable) SignGooglePlay();
    }

    // Подключение к сервисам Google Play
    private static void SignGooglePlay()
    {
        // Подключаемся к сервисам Google Play
        Social.localUser.Authenticate((bool success) => {});
    }

    // Просмотр игровых достижений
    public static void ShowAchievements()
    {
        // Если пользователь вошел в аккаунт, отображаем список достижений
        if (Social.localUser.authenticated) Social.ShowAchievementsUI();
        // Иначе подключаемся к Google Play
        else SignGooglePlay();
    }

    // Разблокировка достижения
    public static void UnlockingAchievement(string identifier)
    {
        // Если пользователь вошел в аккаунт, открываем достижение с указанным идентификатором
        if (Social.localUser.authenticated) Social.ReportProgress(identifier, 100.0f, (bool success) => {});
    }

    // Просмотр таблицы лидеров
    public static void ShowLeaderboard()
    {
        //Если пользователь вошел в аккаунт, отображаем таблицу игроков
        if (Social.localUser.authenticated) PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard);
        // Иначе подключаемся к Google Play
        else SignGooglePlay();
    }

    // Отправка результата в таблицу лидеров
    public static void PostingScoreLeaderboard(int score)
    {
        // Если пользователь вошел в аккаунт, отправляем указанный результат в общую таблицу
        if (Social.localUser.authenticated) Social.ReportScore(score, GPGSIds.leaderboard, (bool success) => { });
    }
}