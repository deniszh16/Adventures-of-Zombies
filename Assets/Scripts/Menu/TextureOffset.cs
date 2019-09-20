using UnityEngine;
using UnityEngine.UI;

public class TextureOffset : MonoBehaviour
{
    [Header("Скорость смещения")]
    [SerializeField] private float speed;

    // Позиция текстуры
    private float position; 

    // Ссылка на графический компонент
    private RawImage image;

    private void Awake() { image = GetComponent<RawImage>(); }

    private void Update()
    {
        // Вычитаем из позиции скорость движения
        position -= (speed * Time.deltaTime);

        // Если позиция текстуры превышает единицу, сбрасываем значение
        if (position > 1.0f) position = -1.0f;

        // Обновляем позицию текстуры
        image.uvRect = new Rect(position, 0, 1, 1);
    }
}