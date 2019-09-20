using UnityEngine;

public class SawHorizontal : Saw
{
    [Header("Скорость движения")]
    [SerializeField] private float speed;

    // Начальный вектор движения пилы
    private Vector3 direction = Vector3.right;

    protected override void Update()
    {
        // Вращаем пилу
        transform.Rotate(Vector3.forward * rotate);
        // Двигаем пилу с указанной скоростью
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, transform.localPosition + direction, speed * Time.deltaTime);

        // Если пила достигает предельных точек, изменяем направление движения
        if (transform.localPosition.x <= -1.7f) direction = Vector3.right;
        else if (transform.localPosition.x >= 1.7f) direction = Vector3.left;
    }
}