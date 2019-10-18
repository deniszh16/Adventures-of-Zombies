using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Активность полета
    private bool flight;

    // Скорость полета стрелы
    private float speed = 15f;

    // Стандартный слой объекта
    private const int layer = 3;

    [Header("Пул для хранения")]
    [SerializeField] private GameObject poolObjects;

    // Направление стрелы
    private Vector3 direction;

    // Ссылки на компоненты
    private BoxCollider2D boxcollider;
    private Rigidbody2D rigbody;
    private SpriteRenderer sprite;
    private FixedJoint2D joint;
    private DamageObjects damage;

    private void Awake()
    {
        boxcollider = GetComponent<BoxCollider2D>();
        rigbody = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        joint = GetComponent<FixedJoint2D>();
        damage = GetComponent<DamageObjects>();
    }

    private void OnEnable()
    {
        // Преобразуем локальный вектор направления в мировой
        direction = transform.TransformDirection(Vector3.down);

        // Активируем полет
        flight = true;

        // Активируем стандартный коллайдер
        boxcollider.enabled = true;
        // Активируем компонент урона
        damage.enabled = true;

        // Устанавливаем слой объекта
        sprite.sortingOrder = layer;

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

            // Отключаем урон под водой
            damage.enabled = false;
        }
        else
        {
            // Пробуем получить компонент урона
            var damage = collision.gameObject.GetComponent<DamageObjects>();

            // Если активно уничтожение стрел
            if (damage && damage.DestroyArrow)
                // Отключаем объект
                gameObject.SetActive(false);
        }
    }

    /// <summary>Остановка полета стрелы (закрепление стрелы на объекте, время до остановки)</summary>
    private IEnumerator FlightStop(bool fixation, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        // Отключаем полет
        flight = false;

        // Если активна фиксация, закрепляем стрелу на объекте
        if (fixation) joint.enabled = true;

        // Отключаем стрелу через указанное время
        Invoke("TurnOffObject", 1.6f);
    }

    /// <summary>Отключение объекта после исползования</summary>
    private void TurnOffObject()
    {
        // Возвращаем стрелу в пул объектов
        gameObject.transform.SetParent(poolObjects.transform);

        // Сбрасываем угол стрелы
        transform.localEulerAngles = Vector3.zero;

        // Отключаем объект
        gameObject.SetActive(false);
    }
}