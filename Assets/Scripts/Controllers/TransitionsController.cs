using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cubra.Controllers
{
    public class TransitionsController : BaseController
    {
        // Перечисление сцен в меню (id сцен)
        public enum Scenes { Menu = 1, Levels, Zombies, Leaderboard }

        // Страница приложения на Google Play
        private const string URL = "https://play.google.com/store/apps/details?id=ru.cubra.zombie";

        /// <summary>
        /// Переход на указанную сцену
        /// </summary>
        /// <param name="scene">сцена для загрузки</param>
        public void GoToScene(int scene)
        {
            SceneManager.LoadScene(scene);
        }

        /// <summary>
        /// Переход на указанную сцену с паузой
        /// </summary>
        /// <param name="seconds">секунды до загрузки</param>
        /// <param name="scene">сцена для загрузки</param>
        public IEnumerator GoToSceneWithPause(float seconds, int scene)
        {
            yield return new WaitForSeconds(seconds);
            GoToScene(scene);
        }

        /// <summary>
        /// Асинхронная загрузка игрового уровня
        /// </summary>
        /// <param name="level">карточка уровня</param>
        public IEnumerator GoToLevel(Level level)
        {
            yield return new WaitForSeconds(1.5f);
            _ = SceneManager.LoadSceneAsync("Level " + level.Number.ToString());
        }

        /// <summary>
        /// Перезагрузка текущей сцены
        /// </summary>
        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /// <summary>
        /// Переход на страницу приложения
        /// </summary>
        public void OpenGamePage()
        {
            Application.OpenURL(URL);
        }

        /// <summary>
        /// Выход из игры
        /// </summary>
        public void ExitGame()
        {
            Application.Quit();
        }
    }
}