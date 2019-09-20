using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Активность полета
    private bool flight;

    // Скорость полета стрелы
    private float speed = 15f;

    [Header("Пул для хранения")]
    [SerializeField] private GameObject poolObjects;

    // Направление стрелы
    private Vector3 direction;

    // Ссылки на компоненты
    private BoxCollider2D boxcollider;
    private Rigidbody2D rigbody;
    private SpriteRenderer sprite;
    private FixedJoint2D joint;

    private void Awake()
    {
        boxcollider = GetComponent<BoxCollider2D>();
        rigbody = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        joint = GetComponent<FixedJoint2D>();
    }

    private void OnEnable()
    {
        // Преобразуем локальный вектор направления в мировой
        direction = transform.TransformDirection(Vector3.down);

        // Активируем полет
        flight = true;

        // Активируем стандартный коллайдер
        boxcollider.enabled = true;

        // Устанавливаем слой объекта
        sprite.sortingOrder = 3;

        // Сбрасываем скорость
        speed = 15f;
    }

    private void FixedUpdate()
    {
        // Если активен полет, выполняется движение в указанном направлении
        if (flight) rigbody.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если стрела касается поверхности
        if (collision.gameObject.tag == "Surface")
        {
            // Отключаем коллайдер
            boxcollider.enabled = false;
            // Понижаем слой объекта
            sprite.sortingOrder--;

            // Назначаем поверхность родительским объектом стрелы
            transform.SetParent(collision.transform);

            // Пробуем получить физический компонент у родительского объекта
            var physics = transform.parent.GetComponent<Rigidbody2D>();

            if (physics)
            {
                // Убираем массу
                rigbody.mass = 0;
                // Закрепляем стрелу на родительском объекте
                joint.connectedBody = physics;
            }

            if (gameObject.activeSelf)
                // Останавливаем полет стрелы через указанное время
                StartCoroutine(FlightStop(physics ? true : false, 0.03f));
        }
        // Если стрела касается воды
        else if (collision.gameObject.GetComponent<Water>())
        {
            // Уменьшаем скорость полета
            speed = 8.5f;
        }
        else
        {
            // Пробуем получить компонент урона
            var damage = collision.gameObject.GetComponent<DamageObjects>();

            // Если активно уничтожение стрел
            if (damage && damage.destroyArrow)
                // Отключаем объект
                gameObject.SetActive(false);
        }
    }

    // Остановка полета стрелы
    private IEnumerator FlightStop(bool fixation, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        // Отключаем полет
        flight = false;

        // Если активна фиксация, закрепляем объект
        if (fixation) joint.enabled = true;

        // Отключаем стрелу через несколько секунды
        Invoke("TurnOffObject", 1.6f);
    }

    // Отключение объекта после исползования
    private void TurnOffObject()
    {
        // Возвращаем объект в общий пул
        gameObject.transform.SetParent(poolObjects.transform);

        // Сбрасываем угол стрелы
        transform.localEulerAngles = Vector3.zero;

        // Отключаем объект
        gameObject.SetActive(false);
    }
}