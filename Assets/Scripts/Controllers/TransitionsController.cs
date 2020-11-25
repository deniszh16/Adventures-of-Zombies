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

        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void OpenGamePage()
        {
            Application.OpenURL(URL);
        }
        
        public void ExitGame()
        {
            Application.Quit();
        }
    }
}