using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Cubra
{
    public class Training : MonoBehaviour
    {
        // Обучение на уровне
        public static bool PlayerTraining = true;

        // Этап обучения
        private int _stage;

        [Header("Ключи подсказок")]
        [SerializeField] private string[] _tips;

        // Ссылка на компоненты подсказок
        private Image _imageHintPanel;

        private void Awake()
        {
            _imageHintPanel = Main.Instance.LevelResults.HintPanel.GetComponent<Image>();
        }

        public IEnumerator StartTraining()
        {
            yield return new WaitForSeconds(1f);
            // Отображаем панель подсказок
            Main.Instance.LevelResults.HintPanel.SetActive(true);

            // Отображаем затемнение экрана
            _imageHintPanel.color = new Color32(0, 0, 0, 230);
            // Активируем обработку нажатий на экран
            _imageHintPanel.raycastTarget = true;

            // Запускаем обучение игрока
            UpdateTrainingText(_tips[0]);
        }

        /// <summary>
        /// Обновление обучающего текста
        /// </summary>
        /// <param name="key">ключ подсказки</param>
        private void UpdateTrainingText(string key)
        {
            // Обновляем текст подсказки
            Main.Instance.LevelResults.TextHint.ChangeKey(key);
            // Увеличиваем этап обучения
            _stage++;
        }

        /// <summary>
        /// Обновление подсказки
        /// </summary>
        public void UpdateHint()
        {
            // Если активен обучающий режим
            if (Main.Instance.CurrentMode == Main.GameModes.Training)
            {
                // Если этап не превышает количество подсказок
                if (_stage < _tips.Length)
                {
                    // Выводим следующую подсказку
                    UpdateTrainingText(_tips[_stage]);
                }
                else
                {
                    // Отключаем повторный показ
                    PlayerTraining = false;

                    // Отключаем обработку нажатий
                    _imageHintPanel.raycastTarget = false;
                    // Скрываем затемнение экрана
                    _imageHintPanel.color = new Color32(0, 0, 0, 0);
                    // Скрываем понель подсказок
                    Main.Instance.LevelResults.HintPanel.SetActive(false);

                    // Включаем управление персонажем
                    Main.Instance.CharacterController.Enable();

                    // Запускаем уровень
                    Main.Instance.LaunchALevel();
                }
            }
        }
    }
}