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
        parameters.interfaceElements[(int)Parameters.InterfaceElements.Blackout].SetActive(true);
        parameters.interfaceElements[(int)Parameters.InterfaceElements.HintPanel].SetActive(true);

        // Активируем обработку нажатий на экран
        parameters.interfaceElements[(int)Parameters.InterfaceElements.HintPanel].GetComponent<Image>().raycastTarget = true;

        // Запускаем обучение игрока
        ShowTraining(tips[0]);
    }

    // Обучение игрока
    private void ShowTraining(string key)
    {
        // Обновляем текст подсказки
        parameters.TextHint.ChangeKey(key);
        // Увеличиваем этап обучения
        stage++;
    }

    // Смена этапа обучения
    public void NextStage()
    {
        // Если активен обучающий режим
        if (parameters.Mode == "training")
        {
            // Если текущий этап меньше общего количества подсказок, обновляем текст
            if (stage < tips.Length) ShowTraining(tips[stage]);
            else
            {
                // Отключаем повторный показ обучения
                display = false;
                // Запускаем уровень
                parameters.RunLevel();
            }
        }
    }
}