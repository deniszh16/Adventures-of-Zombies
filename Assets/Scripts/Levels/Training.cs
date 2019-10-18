using UnityEngine;
using UnityEngine.UI;

public class Training : MonoBehaviour
{
    // Отображение подсказки
    public static bool display = true;

    // Этап обучения
    private int stage = 0;

    [Header("Ключи подсказок")]
    [SerializeField] private string[] tips;

    // Ссылка на параметры
    private Parameters parameters;

    private void Start()
    {
        parameters = Camera.main.GetComponent<Parameters>();

        // Отображаем панель затемнения и панель подсказок
        parameters[(int)Parameters.InterfaceElements.Blackout].SetActive(true);
        parameters[(int)Parameters.InterfaceElements.HintPanel].SetActive(true);

        // Активируем обработку нажатий на экран
        parameters[(int)Parameters.InterfaceElements.HintPanel].GetComponent<Image>().raycastTarget = true;

        // Запускаем обучение игрока
        ShowTraining(tips[0]);
    }

    /// <summary>Обучение игрока (ключ подсказки)</summary>
    private void ShowTraining(string key)
    {
        // Обновляем текст подсказки
        parameters.TextHint.ChangeKey(key);
        // Увеличиваем этап обучения
        stage++;
    }

    /// <summary>Обновление этапа обучения</summary>
    public void NextStage()
    {
        // Если активен обучающий режим
        if (parameters.Mode == "training")
        {
            // Если текущий этап меньше общего количества подсказок
            if (stage < tips.Length)
            {
                // Выводим следующую подсказку
                ShowTraining(tips[stage]);
            }
            else
            {
                // Отключаем повторный показ (при рестарте)
                display = false;

                // Запускаем уровень
                parameters.RunLevel();
            }
        }
    }
}