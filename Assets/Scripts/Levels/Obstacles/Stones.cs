using System.Collections;
using UnityEngine;

public class Stones : MonoBehaviour
{
    // Начальная позиция объекта
    private Vector2 position;

    // Ссылка на компонент физики
    private Rigidbody2D rigbody;

    private void Awake()
    {
        rigbody = GetComponent<Rigidbody2D>();

        // Записываем начальную позицию объекта
        position = transform.position;
    }

    private void OnEnable()
    {
        // Запускаем восстановление объекта
        StartCoroutine(RestoreObject());
    }

    // Восстановление объекта на сцене
    private IEnumerator RestoreObject()
    {
        yield return new WaitForSeconds(5f);

        // Сбрасываем скорость
        rigbody.velocity *= 0;
        // Перемещаем объект к начальной позиции с небольшим смещением
        transform.position = new Vector2(position.x + Random.Range(-3, 3), position.y + Random.Range(-2, 2));
        // Отключаем объект до следующего использования
        gameObject.SetActive(false);
    }
}