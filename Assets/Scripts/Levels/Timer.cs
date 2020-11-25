using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Cubra
{
    public class Timer : MonoBehaviour
    {
        [Header("Секунды на уровень")]
        [SerializeField] private int _seconds;

        public int Seconds => _seconds;

        [Header("Текст таймера")]
        [SerializeField] private Text _textSeconds;

        // Ссылка на обводку текста
        private Outline _textOutline;

        [Header("Время для получения звезд")]
        [SerializeField] private int[] _totalSeconds;

        [Header("Звезды в интерфейсе")]
        [SerializeField] private Image[] _starsSeconds;

        private void Awake()
        {
            _textOutline = _textSeconds.GetComponent<Outline>();
        }

        private void Start()
        {
            GameManager.Instance.LevelLaunched += StartTimer;
        }
        
        public void StartTimer()
        {
            _ = StartCoroutine(TimerCountdown());
        }

        /// <summary>
        /// Таймер уровня
        /// </summary>
        public IEnumerator TimerCountdown()
        {
            while (GameManager.Instance.CurrentMode == GameManager.GameModes.Play)
            {
                if (_seconds > 0)
                {
                    yield return new WaitForSeconds(1);

                    _seconds--;
                    _textSeconds.text = _seconds.ToString();

                    // Если не хватает секунд для текущего количества звезд
                    if (_seconds < _totalSeconds[GameManager.Instance.Stars - 1])
                    {
                        // Уменьшаем текущее количество звезд
                        _starsSeconds[GameManager.Instance.Stars - 1].enabled = false;
                        GameManager.Instance.Stars--;

                        if (GameManager.Instance.Stars == 1)
                        {
                            // Перекрашиваем обводку текста в красный цвет
                            _textOutline.effectColor = new Color32(255, 0, 0, 128);
                        }
                    }
                }
                else
                {
                    GameManager.Instance.CharacterController.DamageToCharacter(true, false);
                    yield break;
                }
            }
        }
    }
}