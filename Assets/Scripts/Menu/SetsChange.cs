using UnityEngine;

public class SetsChange : MonoBehaviour
{
    [Header("Наборы уровней")]
    [SerializeField] private GameObject[] sets;

    private void Start()
    {
        // Активируем набор в зависимости от сохраненного значения
        sets[PlayerPrefs.GetInt("sets") - 1].SetActive(true);

        // Активируем переменную обучения
        Training.display = true;
    }

    /// <summary>Сохранение выбранного набора уровней (номер набора)</summary>
    public void SaveSelectedSet(int number)
    {
        PlayerPrefs.SetInt("sets", number);
    }
}