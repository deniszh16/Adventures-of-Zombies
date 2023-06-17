using System;
using System.Collections;
using UnityEngine;

namespace Logic.Timer
{
    public class Timer : MonoBehaviour
    {
        [Header("Секунды на уровень")]
        [SerializeField] private int _seconds;

        public int Seconds
        {
            get => _seconds;
            set => _seconds = value;
        }

        public int Stars { get; set; } = 3;
        
        public event Action TimerChanged;
        public event Action TimerCompleted;

        private Coroutine _coroutine;

        public void StartTimer() =>
            _coroutine = StartCoroutine(TimerCountdown());

        private IEnumerator TimerCountdown()
        {
            TimerChanged?.Invoke();
            WaitForSeconds second = new WaitForSeconds(1);
            while (_seconds > 0)
            {
                yield return second;
                _seconds--;
                TimerChanged?.Invoke();
            }
            
            if (_seconds <= 0)
                TimerCompleted?.Invoke();
        }

        public void StopTimer() =>
            StopCoroutine(_coroutine);
    }
}