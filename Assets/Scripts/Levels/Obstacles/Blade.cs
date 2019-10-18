using UnityEngine;

public class Blade : MonoBehaviour
{
    // Частота ускорения объекта
    private float time = 3.0f;

    // Ссылка на компонент физики
    private Rigidbody2D rigbody;

    private void Awake()
    {
        rigbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Ускоряем объект каждые три секунды
        InvokeRepeating("AccelerationBlade", time, time);
    }

    /// <summary>Увеличение скорости объекта</summary>
    private void AccelerationBlade()
    {
        // Увеличиваем физическую скорость
        rigbody.velocity *= 1.2f;
    }
}