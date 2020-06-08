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

        // Ссылка на компонент обводки текста
        private Outline _textOutline;

        [Header("Секунды на звезды")]
        [SerializeField] private int[] _totalSeconds;

        [Header("Звезды уровня")]
        [SerializeField] private Image[] _starsSeconds;

        private void Awake()
        {
            _textOutline = _textSeconds.GetComponent<Outline>();
        }

        private void Start()
        {
            Main.Instance.LevelLaunched += StartTimer;
        }

        /// <summary>
        /// Запуск таймера
        /// </summary>
        public void StartTimer()
        {
            _ = StartCoroutine(TimerCountdown());
        }

        /// <summary>
        /// Таймер уровня
        /// </summary>
        public IEnumerator TimerCountdown()
        {
            while (Main.Instance.CurrentMode == Main.GameModes.Play)
            {
                if (_seconds > 0)
                {
                    yield return new WaitForSeconds(1);

                    _seconds--;
                    // Обновляем значение секунд на экране
                    _textSeconds.text = _seconds.ToString();

                    // Если не хватает секунд для текущего количества звезд
                    if (_seconds < _totalSeconds[Main.Instance.Stars - 1])
                    {
                        // Скрываем звездочку
                        _starsSeconds[Main.Instance.Stars - 1].enabled = false;
                        // Уменьшаем текущее количество звезд
                        Main.Instance.Stars--;

                        if (Main.Instance.Stars == 1)
                        {
                            // Перекрашиваем обводку текста в красный цвет
                            _textOutline.effectColor = new Color32(255, 0, 0, 128);
                        }
                    }
                }
                else
                {
                    // Если секунды закончились, уничтожаем персонажа
                    Main.Instance.CharacterController.DamageToCharacter(true, false);
                    yield break;
                }
            }
        }
    }
}