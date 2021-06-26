using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Cubra
{
    public class CountdownToStart : MonoBehaviour
    {
        // Событие по завершению отсчета
        public UnityEvent AfterCountdown;

        [Header("Количество секунд")]
        [SerializeField] private int _countdown = 3;

        private Text _textComponent;

        private void Awake()
        {
            _textComponent = GetComponent<Text>();
        }

        /// <summary>
        /// Отсчет времени до начала уровня
        /// </summary>
        public IEnumerator StartCountdown()
        {
            var second = new WaitForSeconds(1);

            while (_countdown > 0)
            {
                _textComponent.text = _countdown.ToString();
                yield return second;
                _countdown--;
            }

            AfterCountdown?.Invoke();

            _countdown = 3;
            gameObject.SetActive(false);
        }
    }
}