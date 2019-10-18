using UnityEngine;

public class MoveToPoints : MonoBehaviour
{
    [Header("Отображение спрайта")]
    [SerializeField] private bool flipSprite = false;

    [Header("Скорость движения")]
    [SerializeField] private float speed = 1.0f;

    // Активная точка
    private int point = 0;

    [Header("Точки для движения платформы")]
    [SerializeField] private Vector3[] points;

    // Ссылка на графический компонент
    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        // Если текущая позиция не равна позиции активной точки
        if (transform.position != points[point])
            // Двигаем объект до позиции активной точки
            transform.position = Vector3.MoveTowards(transform.position, points[point], speed * Time.deltaTime);
        else
        {
            // При необходимости поворачиваем спрайт
            if (flipSprite) sprite.flipX = !sprite.flipX;

            // Переключаемся на другую точку, либо сбрасываем ее
            point = (point < points.Length - 1) ? ++point : 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Получаем компонент персонажа у конувшегося объекта
        var character = collision.gameObject.GetComponent<Character>();

        if (character)
            // Назначаем персонажу родительским объектом платформу
            character.transform.parent = gameObject.transform;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        var character = collision.gameObject.GetComponent<Character>();

        if (character)
            // Сбрасываем родительский объект для персонажа
            character.transform.parent = null;
    }
}