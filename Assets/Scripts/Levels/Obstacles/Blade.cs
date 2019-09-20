using UnityEngine;

public class Blade : MonoBehaviour
{
    // Частота ускорения объекта
    private float time = 3.0f;

    private Rigidbody2D rigbody;

    private void Awake() { rigbody = GetComponent<Rigidbody2D>(); }

    private void Start()
    {
        // Ускорение объекта каждые три секунды
        InvokeRepeating("AccelerationBlade", time, time);
    }

    // Увеличение объекта
    private void AccelerationBlade()
    {
        // Увеличиваем физическую скорость
        rigbody.velocity *= 1.2f;
    }
}