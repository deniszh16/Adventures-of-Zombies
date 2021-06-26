using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Cubra
{
    public class Training : MonoBehaviour
    {
        // Обучение на текущем уровне
        public static bool PlayerTraining = true;

        // Этап обучения
        private int _stage;

        [Header("Ключи подсказок")]
        [SerializeField] private string[] _tips;

        private Image _imageHintPanel;

        private void Start()
        {
            _imageHintPanel = GameManager.Instance.LevelResults.HintPanel.GetComponent<Image>();
        }
        
        public IEnumerator StartTraining()
        {
            yield return new WaitForSeconds(1f);

            GameManager.Instance.LevelResults.HintPanel.SetActive(true);
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
            GameManager.Instance.LevelResults.TextHint.ChangeKey(key);
            _stage++;
        }

        /// <summary>
        /// Обновление подсказки
        /// </summary>
        public void UpdateHint()
        {
            if (GameManager.Instance.CurrentMode == GameManager.GameModes.Training)
            {
                if (_stage < _tips.Length)
                {
                    UpdateTrainingText(_tips[_stage]);
                }
                else
                {
                    PlayerTraining = false;

                    _imageHintPanel.raycastTarget = false;
                    _imageHintPanel.color = new Color32(0, 0, 0, 0);
                    GameManager.Instance.LevelResults.HintPanel.SetActive(false);

                    if (GameManager.Instance.Countdown != null)
                    {
                        _ = StartCoroutine(GameManager.Instance.Countdown.StartCountdown());
                    }
                    else
                    {
                        GameManager.Instance.CharacterController.Enable();
                        GameManager.Instance.LaunchALevel();
                    }
                }
            }
        }
    }
}