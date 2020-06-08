using UnityEngine;

namespace Cubra
{
    public class Pause : MonoBehaviour
    {
        /// <summary>
        /// Переключение игровой паузы
        /// </summary>
        /// <param name="pause">пауза в игре</param>
        public void TogglePause(bool pause)
        {
            Time.timeScale = pause ? 0 : 1;
        }
    }
}