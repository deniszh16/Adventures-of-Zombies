using UnityEngine;
using UnityEngine.UI;

public class StarsPerLevel : MonoBehaviour
{
    [Header("Номер уровня")]
    [SerializeField] private int number;

    [Header("Спрайты звезд")]
    [SerializeField] private Sprite[] sprites = new Sprite[3];

    // Объект для работы json по звездам
    private StarsJson Stars { get; set; } = new StarsJson();

    private Image image;

    private void Awake() { image = GetComponent<Image>(); }

    private void Start()
    {
        // Преобразование сохраненной json строки в объект
        Stars = JsonUtility.FromJson<StarsJson>(PlayerPrefs.GetString("stars-level"));

        // Если номер уровня меньше количества сохраненных значений
        if (number <= Stars.stars.Count)
            // Устанавливаем спрайт звезд из массива
            image.sprite = sprites[Stars.stars[number - 1] - 1];
    }
}