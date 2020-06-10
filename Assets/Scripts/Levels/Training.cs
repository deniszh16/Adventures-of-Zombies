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

        /// <summary>
        /// Запускаем обучение игрока
        /// </summary>
        public IEnumerator StartTraining()
        {
            yield return new WaitForSeconds(1f);

            Main.Instance.LevelResults.HintPanel.SetActive(true);
            _imageHintPanel.color = new Color32(0, 0, 0, 230);
            _imageHintPanel.raycastTarget = true;

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
            _stage++;
        }

        /// <summary>
        /// Обновление подсказки
        /// </summary>
        public void UpdateHint()
        {
            if (Main.Instance.CurrentMode == Main.GameModes.Training)
            {
                if (_stage < _tips.Length)
                {
                    // Выводим следующую подсказку
                    UpdateTrainingText(_tips[_stage]);
                }
                else
                {
                    // Отключаем повторный показ
                    PlayerTraining = false;

                    _imageHintPanel.raycastTarget = false;
                    _imageHintPanel.color = new Color32(0, 0, 0, 0);
                    Main.Instance.LevelResults.HintPanel.SetActive(false);

                    Main.Instance.CharacterController.Enable();
                    Main.Instance.LaunchALevel();
                }
            }
        }
    }
}