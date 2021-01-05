using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

namespace Cubra
{
    public class Leaderboard : MonoBehaviour
    {
        [Header("Ранг игрока")]
        [SerializeField] private Text _playerRank;

        [Header("Счет игрока")]
        [SerializeField] private Text _playerScore;

        [Header("Лучшие игроки")]
        [SerializeField] private Text _leaderboard;

        [Header("Загрузка результатов")]
        [SerializeField] private GameObject _loading;

        [Header("Компонент скролла")]
        [SerializeField] private ScrollRect _scrollRect;

        private void Start()
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                // Отправляем свой результат в таблицу лидеров
                GooglePlayServices.PostingScoreLeaderboard(PlayerPrefs.GetInt("score") + PlayerPrefs.GetInt("total-coins"));

                // Если пользователь авторизирован, загружаем результаты
                if (GooglePlayServices.Authenticated) LoadScoresLeaderboard();
            }
            else
            {
                ShowSavedResults();
            }

            _playerScore.text = "(" + PlayerPrefs.GetInt("score") + ")";
        }

        /// <summary>
        /// Загрузка результатов из удаленной таблицы лидеров
        /// </summary>
        public void LoadScoresLeaderboard()
        {
            // Загружаем десять лучших результатов из публичной таблицы
            PlayGamesPlatform.Instance.LoadScores(
                GPGSIds.leaderboard,
                LeaderboardStart.TopScores,
                10,
                LeaderboardCollection.Public,
                LeaderboardTimeSpan.AllTime,
                (data) =>
                {
                    // Обновляем перевод в текстовом поле
                    _playerRank.GetComponent<TextTranslation>().ChangeKey("rank-description");
                    _playerRank.text = data.PlayerScore.rank.ToString() + "\n" + _playerRank.text;

                    // Сохраняем ранг игрока
                    PlayerPrefs.SetInt("rank", data.PlayerScore.rank);

                    // Загружаем информацию по остальным игрокам
                    LoadUsersLeaderboard(data.Scores);
            });
        }

        /// <summary>
        /// Загрузка информации по игрокам
        /// </summary>
        /// <param name="scores">массив лучших результатов</param>
        private void LoadUsersLeaderboard(IScore[] scores)
        {
            // Список из id пользователей
            List<string> userIds = new List<string>();

            // Перебираем результаты, заполняем список идентификаторами
            foreach (IScore score in scores) { userIds.Add(score.userID); }

            Social.LoadUsers(userIds.ToArray(), (users) =>
            {
                // Скрываем значок загрузки
                _loading.SetActive(false);

                foreach (IScore score in scores)
                {
                    // Создаем пользователя и ищем его id массиве
                    IUserProfile user = FindUser(users, score.userID);

                    // Выводим в текстовое поле ранг, имя и счет игрока
                    _leaderboard.text = score.rank + " - " + ((user != null) ? user.userName : "Unknown") + " (" + score.value + ")\n\n";
                }

                _scrollRect.verticalNormalizedPosition = 1;
                PlayerPrefs.SetString("gp-leaderboard", _leaderboard.text);

            });
        }

        /// <summary>
        /// Поиск игрока в загруженном списке профилей
        /// </summary>
        /// <param name="users">массив профилей</param>
        /// <param name="userid">идентификатор игрока</param>
        private IUserProfile FindUser(IUserProfile[] users, string userid)
        {
            foreach (IUserProfile user in users)
            {
                // Если id совпадают, возвращаем найденного игрока
                if (user.id == userid) return user;
            }

            return null;
        }

        /// <summary>
        /// Отображение сохраненных результатов по игрокам
        /// </summary>
        private void ShowSavedResults()
        {
            // Позиция игрока в рейтинге
            var rank = PlayerPrefs.GetInt("rank");

            if (rank > 0)
            {
                // Обновляем перевод в текстовом поле
                _playerRank.GetComponent<TextTranslation>().ChangeKey("rank-description");
                _playerRank.text = rank + "\n" + _playerRank.text;
            }

            // Список лучших игроков
            var leaders = PlayerPrefs.GetString("gp-leaderboard");

            if (leaders.Length > 0)
            {
                // Скрываем загрузку
                _loading.SetActive(false);

                // Выводим список лидеров
                _leaderboard.text = leaders;
            }
        }
    }
}