using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class Leaderboard : MonoBehaviour
{
    [Header("Персонаж игрока")]
    [SerializeField] private SpriteRenderer character;

    [Header("Изображения персонажей")]
    [SerializeField] private Sprite[] characters;

    [Header("Ранг игрока")]
    [SerializeField] private Text playerRank;

    [Header("Счет игрока")]
    [SerializeField] private Text playerScore;

    [Header("Лучшие игроки")]
    [SerializeField] private Text bestPlayers;

    // Анимация текста
    private Animator animator;

    [Header("Компонент скролла")]
    [SerializeField] private ScrollRect scroll;

    private void Awake()
    {
        character = character.GetComponent<SpriteRenderer>();
        animator = bestPlayers.gameObject.GetComponent<Animator>();
    }

    private void Start()
    {
        // Устанавливаем изображение активного зомби
        character.sprite = characters[PlayerPrefs.GetInt("character") - 1];

        // Если интернет доступен
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            // Если пользователь вошел в аккаунт, загружаем результаты
            if (Social.localUser.authenticated) LoadScoresLeaderboard();
        }
        else
        {
            // Получаем сохраненный ранг игрока
            int rank = PlayerPrefs.GetInt("rank");

            if (rank > 0)
                // Если ранг больше нуля, выводим его в текстовове поле
                playerRank.text = rank.ToString() + ParseTranslation.languages.Element("languages").Element(Options.language).Element("rank-description").Value;

            // Выводим счет игрока
            playerScore.text = "(" + PlayerPrefs.GetInt("score").ToString() + ")";
        }
    }

    // Загрузка результатов из таблицы лидеров
    public void LoadScoresLeaderboard()
    {
        // Отображаем текст загрузки
        bestPlayers.text = ParseTranslation.languages.Element("languages").Element(Options.language).Element("loading-results").Value;
        // Активируем анимацию загрузки
        animator.SetBool("Loading", true);

        // Загружаем десять лучших результатов из публичной таблицы
        PlayGamesPlatform.Instance.LoadScores(
            GPGSIds.leaderboard,
            LeaderboardStart.TopScores,
            10,
            LeaderboardCollection.Public,
            LeaderboardTimeSpan.AllTime,
            (data) =>
            {
                // Выводим ранг текущего игрока
                playerRank.text = data.PlayerScore.rank.ToString() + "\n" + ParseTranslation.languages.Element("languages").Element(Options.language).Element("rank-description").Value; ;
                // Сохраняем ранг игрока
                PlayerPrefs.SetInt("rank", data.PlayerScore.rank);

                // Выводим счет текущего игрока
                playerScore.text = "(" + data.PlayerScore.formattedValue + ")";

                // Загружаем информацию по остальным игрокам
                LoadUsersLeaderboard(data.Scores);
            });
    }

    // Загрузка и отображение информации по игрокам
    private void LoadUsersLeaderboard(IScore[] scores)
    {
        // Создаем список из id пользователей
        List<string> userIds = new List<string>();

        // Перебираем результаты в массиве и добавляем в список id игроков
        foreach (IScore score in scores) { userIds.Add(score.userID); }

        // Загружаем информацию по пользователям
        Social.LoadUsers(userIds.ToArray(), (users) =>
        {
            // Отключаем анимацию загрузки
            animator.SetBool("Loading", false);
            // Сбрасываем текст
            bestPlayers.text = "\n";

            // Перебираем результаты в массиве
            foreach (IScore score in scores)
            {
                // Создаем пользователя и вызываем поиск его id массиве
                IUserProfile user = FindUser(users, score.userID);

                // Выводим в текстовое поле ранг, имя и счет игрока
                bestPlayers.text += score.rank + " - " + ((user != null) ? user.userName : "Unknown") + " (" + score.value + ")\n\n";
            }

            // Перемещаем скролл вверх
            scroll.verticalNormalizedPosition = 1;
        });
    }

    // Поиск игрока в массиве по id
    private IUserProfile FindUser(IUserProfile[] users, string userid)
    {
        // Переборка игроков в массиве
        foreach (IUserProfile user in users)
        {
            // Если id совпадают, возвращаем найденного игрока
            if (user.id == userid) return user;
        }

        // Иначе возвращаем null
        return null;
    }
}