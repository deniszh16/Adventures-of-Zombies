using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionsInMenu : MonoBehaviour
{
    [Header("Кнопка возврата")]
    [SerializeField] private bool buttonBack;

    [Header("Сцена для возврата")]
    [SerializeField] private int scene;

    // Страница приложения на Google Play
    private const string url = "https://play.google.com/store/apps/details?id=ru.cubra.zombie";

    private void Update()
    {
        // При нажатии кнопки возврата, выполняется переход на указанную сцену
        if (buttonBack && Input.GetKeyDown(KeyCode.Escape) ) SceneManager.LoadScene(scene);
    }

    /// <summary>Переход на указанную сцену (номер сцены)</summary>
    public void GoToScene(int number) { SceneManager.LoadScene(number); }

    /// <summary>Переход на указанную сцену (текстовое название сцены)</summary>
    public void GoToScene(string name) { SceneManager.LoadScene(name); }

    /// <summary>Перезапуск текущей сцены</summary>
    public void RestartLevel() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }

    /// <summary>Переход на страницу приложения</summary>
    public void LeaveFeedback() { Application.OpenURL(url); }

    /// <summary>Просмотр достижений Google Play</summary>
    public void ViewAchievements() { PlayServices.ShowAchievements(); }

    /// <summary>Просмотр таблицы лидеров Google Play</summary>
    public void ViewLeaderboard() { PlayServices.ShowLeaderboard(); }

    /// <summary>Выход из игры</summary>
    public void ExitGame() { Application.Quit(); }
}