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

    // Переход на указанную сцену (по номеру)
    public void GoToScene(int number) { SceneManager.LoadScene(number); }

    // Переход на указанную сцену (по названию)
    public void GoToScene(string name) { SceneManager.LoadScene(name); }

    // Перезапуск уровня
    public void RestartLevel() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }

    // Переход на страницу приложения
    public void LeaveFeedback() { Application.OpenURL(url); }

    // Просмотр достижений Google Play
    public void ViewAchievements() { PlayServices.ShowAchievements(); }

    // Просмотр таблицы лидеров Google Play
    public void ViewLeaderboard() { PlayServices.ShowLeaderboard(); }

    // Выход из игры
    public void ExitGame() { Application.Quit(); }
}