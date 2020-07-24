using UnityEngine;

namespace Cubra
{
    public class ChoiceSets : MonoBehaviour
    {
        // Нахождение на игровом уровне
        public static bool AtLevel;

        [Header("Наборы уровней")]
        [SerializeField] private GameObject[] _sets;

        private void Start()
        {
            // Активируем выбранный (сохраненный) набор уровней
            _sets[PlayerPrefs.GetInt("sets") - 1].SetActive(true);
            // Активируем обучение на уровнях
            Training.PlayerTraining = true;
        }

        /// <summary>
        /// Сохранение выбранного набора уровней
        /// </summary>
        /// <param name="number">номер набора</param>
        public void SaveSet(int number)
        {
            PlayerPrefs.SetInt("sets", number);
        }
    }
}