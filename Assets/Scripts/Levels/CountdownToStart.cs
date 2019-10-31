using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CountdownToStart : MonoBehaviour
{
    // Событие по завершению отсчета
    public UnityEvent AfterCountdown { get; } = new UnityEvent();

    [Header("Количество секунд")]
    [SerializeField] private int countdown = 3;

    // Ссылки на компоненты
    private Text textComponent;
    private Parameters parameters;

    private void Awake()
    {
        textComponent = GetComponent<Text>();
        parameters = Camera.main.GetComponent<Parameters>();

        // Подписываем запуск отсчета в событие по запуску уровня
        parameters.StartLevel.AddListener(StartCountdown);
    }

    /// <summary>Запуск отсчета</summary>
    private void StartCountdown()
    {
        StartCoroutine(Countdown());
    }

    /// <summary>Отсчет времени до начала уровня</summary>
    private IEnumerator Countdown()
    {
        while (countdown > 0)
        {
            // Обновляем текст отсчета
            textComponent.text = countdown.ToString();

            yield return new WaitForSeconds(1.0f);
            // Уменьшаем секунды
            countdown--;
        }

        // Вызываем зарегистрированные методы
        AfterCountdown?.Invoke();

        // Скрываем текст таймера
        gameObject.SetActive(false);
    }
}