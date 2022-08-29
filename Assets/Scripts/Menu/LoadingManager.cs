using UnityEngine;
using Cubra.Controllers;

namespace Cubra
{
    public class LoadingManager : MonoBehaviour
    {
        private void Awake()
        {
            #region Saved Data
            if (PlayerPrefs.HasKey("saved-data") == false)
            {
                // Язык интерфейса игры
                PlayerPrefs.SetString("current-language", (Application.systemLanguage == SystemLanguage.Russian) ? "ru-RU" : "en-US");
                // Настройка звука
                PlayerPrefs.SetInt("sounds", 0);

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

                // Первоначальные сохранения
                PlayerPrefs.SetString("saved-data", "yes");
            }
            #endregion
        }

        private void Start()
        {
            var transitions = gameObject.GetComponent<TransitionsController>();
            _ = transitions.StartCoroutine(transitions.GoToSceneWithPause(1.2f, (int)TransitionsController.Scenes.Menu));
        }
    }
}