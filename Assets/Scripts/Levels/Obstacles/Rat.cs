using UnityEngine;

public class Rat : MonoBehaviour
{
    [Header("Ограничители для крысы")]
    [SerializeField] private Transform[] limiters;

    [Header("Скорость движения")]
    [SerializeField] private float speed;

    // Дальность обнаружения
    private Vector2 distance;

    // Направление движения крысы
    private Vector2 direction = Vector2.left;

    // Цель для крысиных атак
    private GameObject target;

    // Перечисление анимаций крысы
    private enum RatAnimations { Idle, Run, Attack }

    // Установка параметра в аниматоре
    private RatAnimations State { set { animator.SetInteger("State", (int)value); } }

    // Ссылки на компоненты
    private SpriteRenderer sprite;
    private Animator animator;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Если цель отсутствует
        if (!target)
        {
            // Сбрасываем анимацию
            State = RatAnimations.Idle;

            // Меняем направление рейкаста
            direction *= -1;

            // Определяем дистанцию (до ограничителей) для рейкаста
            distance.x = (direction.x > 0) ? limiters[1].localPosition.x - transform.localPosition.x : transform.localPosition.x - limiters[0].localPosition.x;

            // Выполняем рейкаст в указанном направлении
            RaycastHit2D hit = Physics2D.Raycast(transform.localPosition, direction, distance.x);

            // Если найден коллайдер
            if (hit.collider)
            {
                // Пытаемся у него получить компонент персонажа
                var character = hit.collider.gameObject.GetComponent<Character>();

                if (character)
                    // Записываем персонажа в цель
                    target = character.gameObject;
            }
        }
        else
        {
            // Определяем дистанцию между крысой и целью
            distance = transform.localPosition - target.transform.position;

            // Если вертикальная дистанция в указанном диапазоне
            if (Mathf.Abs(distance.y) < 3.8f)
            {
                // Отображаем спрайт в зависимости от дистанции
                sprite.flipX = (distance.x > 0) ? false : true;

                // Если крыса не заходит за ограничители, выполняем движение
                if (distance.x > 0 && transform.localPosition.x > limiters[0].localPosition.x ||
                    distance.x < 0 && transform.localPosition.x < limiters[1].localPosition.x) Run();
                // Иначе сбрасываем анимацию
                else State = RatAnimations.Idle;
            }
            // Иначе сбрасываем цель
            else target = null;
        }
    }

    /// <summary>Бег крысы</summary>
    private void Run()
    {
        // Меняем анимацию на бег
        State = RatAnimations.Run;

        // Выполняем движение крысы к цели
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), speed * Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Пытаемся получить компонент персонажа у касающегося объекта
        var character = collision.gameObject.GetComponent<Character>();

        // Если персонаж живой
        if (character.Life)
        {
            // Устанавливаем атакующую анимацию
            State = RatAnimations.Attack;

            // Отображаем эффект урона
            character.ShowDamageEffect();
            // Наносим урон персонажу без отскока и с анимацией смерти
            character.RecieveDamageCharacter(false, true, 1.5f);
        }
    }
}