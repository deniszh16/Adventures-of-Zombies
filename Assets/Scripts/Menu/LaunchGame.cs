using UnityEngine;
using UnityEngine.SceneManagement;
using GooglePlayGames;

public class LaunchGame : MonoBehaviour
{
    private void Awake()
    {
        // Перевод интерфейса игры
        if (!PlayerPrefs.HasKey("language"))
            PlayerPrefs.SetString("language", (Application.systemLanguage == SystemLanguage.Russian) ? "ru" : "en");

        // Настройка звука
        if (!PlayerPrefs.HasKey("sounds")) PlayerPrefs.SetString("sounds", "true");

        // Основной прогресс игры
        if (!PlayerPrefs.HasKey("progress")) PlayerPrefs.SetInt("progress", 1);

        // Общий счет игры
        if (!PlayerPrefs.HasKey("score")) PlayerPrefs.SetInt("score", 0);

        // Общее количество монет
        if (!PlayerPrefs.HasKey("coins")) PlayerPrefs.SetInt("coins", 0);

        // Общее количество монет за всё время
        if (!PlayerPrefs.HasKey("piggybank")) PlayerPrefs.SetInt("piggybank", 0);

        // Общее количество собранных мозгов
        if (!PlayerPrefs.HasKey("brains")) PlayerPrefs.SetInt("brains", 0);

        // Выбранный набор уровней
        if (!PlayerPrefs.HasKey("sets")) PlayerPrefs.SetInt("sets", 1);

        // Позиция игрока в таблице лидеров
        if (!PlayerPrefs.HasKey("rank")) PlayerPrefs.SetInt("rank", 0);

        // Номер выбранного персонажа
        if (!PlayerPrefs.HasKey("character")) PlayerPrefs.SetInt("character", 1);

        // Статистика по персонажам
        if (!PlayerPrefs.HasKey("character-1"))
        {
            PlayerPrefs.SetString("character-1", "{\"played\": 0, \"loss\": 0}");
            PlayerPrefs.SetString("character-2", "{\"played\": 0, \"loss\": 0}");
            PlayerPrefs.SetString("character-3", "{\"played\": 0, \"loss\": 0}");
            PlayerPrefs.SetString("character-4", "{\"played\": 0, \"loss\": 0}");
            PlayerPrefs.SetString("character-5", "{\"played\": 0, \"loss\": 0}");
        }

        // Звезды за пройденные уровни
        if (!PlayerPrefs.HasKey("stars-level")) PlayerPrefs.SetString("stars-level", "{\"stars\": []}");

        // Общее количество уничтоженных бочек
        if (!PlayerPrefs.HasKey("barrel")) PlayerPrefs.SetInt("barrel", 0);

        // Активация игровых сервисов Google Play
        PlayGamesPlatform.Activate();

        // Переход в главное меню
        Invoke("GoToMenu", 2.0f);
    }

    /// <summary>Переход в главное меню</summary>
    private void GoToMenu() { SceneManager.LoadScene(1); }
}