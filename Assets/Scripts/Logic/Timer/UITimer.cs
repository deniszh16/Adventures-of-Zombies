using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Logic.Timer
{
    public class UITimer : MonoBehaviour
    {
        [Header("Компонент таймера")]
        [SerializeField] private Timer _timer;
        
        [Header("Текст таймера")]
        [SerializeField] private TextMeshProUGUI _textSeconds;

        [Header("Время на звезды")]
        [SerializeField] private int[] _totalSeconds;

        [Header("Звезды таймера")]
        [SerializeField] private Image[] _starsSeconds;

        private Color _сhangedСolor = new Color(255, 0, 0, 128);

        private void Awake() =>
            _timer.TimerChanged += UpdateTimerCounter;

        private void UpdateTimerCounter()
        {
            _textSeconds.text = _timer.Seconds.ToString();

            if (_timer.Seconds < _totalSeconds[_timer.Stars - 1])
            {
                _starsSeconds[_timer.Stars - 1].enabled = false;
                _timer.Stars--;
                
                if (_timer.Stars == 1)
                    _textSeconds.color = _сhangedСolor;
            }
        }

        private void OnDestroy() =>
            _timer.TimerChanged -= UpdateTimerCounter;
    }
}