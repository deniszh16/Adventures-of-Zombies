using UnityEngine;

namespace Cubra
{
    public class Resurrect : MonoBehaviour
    {
        /// <summary>
        /// Воскрешение персонажа
        /// </summary>
        public void ResurrectCharacter()
        {
            if (PlayerPrefs.GetInt("coins") >= 50)
            {
                PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - 50);
                GameManager.Instance.LevelResults.ResumeLevel();
            }
        }
    }
}