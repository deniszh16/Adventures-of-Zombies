using UnityEngine;
using UnityEngine.UI;

public class ParallaxBackground : MonoBehaviour
{
    [Header("Скорость параллакса")]
    [SerializeField] private float speed;

    // Позиция текстуры
    private float position;

    // Предыдущая позиция камеры
    private Vector3 pastPosition;

    // Ссылки на компоненты
    private RawImage image;
    private CharacterControl control;

    private void Awake()
    {
        image = GetComponent<RawImage>();
        control = GameObject.FindGameObjectWithTag("Controls").GetComponent<CharacterControl>();
    }

    private void Start()
    {
        // Записываем последнюю позицию камеры
        pastPosition.x = Camera.main.transform.position.x;
    }

    private void Update()
    {
        // Если последняя округленная позиция камеры не равна текущей
        if ((int)(pastPosition.x * 10) / 10.0f != (int)(Camera.main.transform.position.x * 10) / 10.0f)
        {
            // Обновляем позицию для текстуры
            position += control.Vector.x * speed * Time.deltaTime;

            // Если позиция текстуры больше единицы, сбрасываем позицию
            if (position > 1.0f) position = -1.0f;

            // Обновляем позицию текстуры
            image.uvRect = new Rect(position, 0, 1, 1);
        }

        // Обновляем последнюю позицию камеры
        pastPosition.x = Camera.main.transform.position.x;
    }
}