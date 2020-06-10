using UnityEngine;
using Cubra.Controllers;
using GooglePlayGames;

namespace Cubra
{
    public class LoadingManager : MonoBehaviour
    {
        private void Awake()
        {
            #region Saved Data
            // Если отсутствуют сохранения, устанавливаем значения по умолчанию
            if (PlayerPrefs.HasKey("saved-data") == false)
            {
                // Язык интерфейса игры
                PlayerPrefs.SetString("current-language", (Application.systemLanguage == SystemLanguage.Russian) ? "ru-RU" : "en-US");
                // Настройка звука
                PlayerPrefs.SetInt("sounds", 0);

                // Активный набор уровней
                PlayerPrefs.SetInt("sets", 1);
                // Звезды за пройденные уровни
                PlayerPrefs.SetString("stars-level", "{\"Stars\": []}");
                // Активный игровой персонаж
                PlayerPrefs.SetInt("character", 1);

                // Статистика по персонажам
                PlayerPrefs.SetString("character-1", "{\"Played\": 0, \"Deaths\": 0}");
                PlayerPrefs.SetString("character-2", "{\"Played\": 0, \"Deaths\": 0}");
                PlayerPrefs.SetString("character-3", "{\"Played\": 0, \"Deaths\": 0}");
                PlayerPrefs.SetString("character-4", "{\"Played\": 0, \"Deaths\": 0}");
                PlayerPrefs.SetString("character-5", "{\"Played\": 0, \"Deaths\": 0}");

                // Прогресс викторины
                PlayerPrefs.SetInt("progress", 1);
                // Общий игровой счет
                PlayerPrefs.SetInt("score", 0);
                // Текущее количество монет
                PlayerPrefs.SetInt("coins", 0);
                // Общее количество собранных монет
                PlayerPrefs.SetInt("total-coins", 0);
                // Общее количество собранных мозгов
                PlayerPrefs.SetInt("brains", 0);
                // Общее количество уничтоженных бочек
                PlayerPrefs.SetInt("barrel", 0);

                // Позиция игрока в рейтинге
                PlayerPrefs.SetInt("rank", 0);
                // Таблица лучших игроков
                PlayerPrefs.SetString("gp-leaderboard", "");

                // Запись сохранений
                PlayerPrefs.SetString("saved-data", "true");
            }
            #endregion

            // Активация игровых сервисов Google Play
            _ = PlayGamesPlatform.Activate();
        }

        private void Start()
        {
            var transitions = gameObject.GetComponent<TransitionsController>();
            _ = transitions.StartCoroutine(transitions.GoToSceneWithPause(2f, (int)TransitionsController.Scenes.Menu));
        }
    }
}