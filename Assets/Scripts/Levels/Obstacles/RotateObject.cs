using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [Header("Скорость поворота")]
    [SerializeField] private float speed = 2f;

    [Header("Пауза между поворотами")]
    [SerializeField] private float pause = 2.5f;

    // Угол для поворота
    private int angle = 0;

    private void Start()
    {
        // Запускаем повороты объекта
        InvokeRepeating("ChangeAngle", 1.0f, pause);
    }

    private void ChangeAngle()
    {
        // Увеличиваем угол
        angle += 90;
    }

    private void Update()
    {
        // Если текущий угол нулевой, а переменная набрала полный оборот
        if ((int)transform.localEulerAngles.z == 0 && angle >= 360)
            // Обнуляем угол
            angle = 0;

        // Если текущий угол меньше переменной угла
        if (transform.localEulerAngles.z < angle)
            // Выполняем поворот объекта
            transform.Rotate(Vector3.forward, speed);
    }
}