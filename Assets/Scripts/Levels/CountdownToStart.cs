using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountdownToStart : MonoBehaviour
{
    [Header("Секунды до начала")]
    [SerializeField] private int countdown = 3;

    // Ссылки на компоненты
    private Text textComponent;
    private Parameters parameters;

    private void Awake()
    {
        textComponent = GetComponent<Text>();
        parameters = Camera.main.GetComponent<Parameters>();

        // Добавляем метод в событие по запуску уровня
        parameters.StartLevel.AddListener(StartCountdown);
    }

    private void StartCountdown()
    {
        // Запускаем отсчет до начала
        StartCoroutine(Countdown());
    }

    // Отсчет времени до старта
    private IEnumerator Countdown()
    {
        while (countdown > 0)
        {
            // Обновляем текст отсчета
            textComponent.text = countdown.ToString();

            // Отсчитываем секунду
            yield return new WaitForSeconds(1.0f);
            // Уменьшаем количество секунд
            countdown--;
        }

        // Активируем босса на уровне
        FindObjectOfType<Boss>().ActivateBoss();

        // Скрываем текст таймера
        gameObject.SetActive(false);
    }
}