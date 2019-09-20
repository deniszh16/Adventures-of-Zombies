using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("Пауза для переходов")]
    [SerializeField] private float pause = 2f;

    [Header("Точки переходов")]
    [SerializeField] private Vector3[] points;

    // Активная точка
    private int activePoint = 0;

    private void Start()
    {
        // Запускаем переходы платформы
        InvokeRepeating("TeleportPlatform", pause, pause);
    }

    // Телепортация платформы
    private void TeleportPlatform()
    {
        // Увеличиваем номер точки
        activePoint++;

        // Если точка вышла за пределы массива
        if (activePoint > (points.Length - 1))
            // Обнуляем точку
            activePoint = 0;

        // Выполняем переход платформы к активной точке
        transform.position = points[activePoint];
    }
}