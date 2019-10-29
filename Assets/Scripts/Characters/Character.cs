using UnityEngine;

public class Character : MonoBehaviour
{
    // Жизнь персонажа
    public bool Life { get; set; } = true;

    // Скорость движения персонажа
    public float Speed { get; set; } = speed;

    // Стандартная скорость движения
    private const float speed = 7.5f;

    // Высота прыжка персонажа
    private float JumpForce { get; } = 12f;

    // Проверка на прыжок персонажа
    private bool IsJumping { get; set; } = false;

    // Проверка нахождения на земле
    private bool IsGrounded { get; set; } = false;

    // Позиция коллайдера персонажа
    private Vector2 Point { get; set; }

    // Смещение коллайдера персонажа
    private Vector3 Offset { get; } = new Vector2(0.1f, 1.0f);

    // Размер коллайдера персонажа
    private Vector2 Size { get; } = new Vector2(0.5f, 0.75f);

    // Проверка на вис на крюке
    public bool IsHook { get; set; } = false;

    // Перечисление анимаций персонажа
    private enum CharacterAnimations { Idle, Run, Jump, Dead, Hook }

    // Переключение анимаций персонажа
    private CharacterAnimations State { set { animator.SetInteger("State", (int)value); } }

    // Стандартный слой персонажа на сцене
    private const int layer = 5;

    // Позиция респауна персонажа
    public Vector3 RespawnPosition { get; set; }

    [Header("Эффект урона")]
    [SerializeField] private ParticleSystem blood;

    // Ссылки на компоненты персонажа
    public SpriteRenderer Sprite { get; private set; }
    public Rigidbody2D Rigbody { get; private set; }

    private PolygonCollider2D polCollider;
    private Animator animator;

    // Ссылки на управление персонажем, параметры и звук
    public CharacterControl Control { get; private set; }
    public Parameters Parameters { get; private set; }
    public AudioSource AudioSource { get; private set; }

    private void Awake()
    {
        Sprite = GetComponentInChildren<SpriteRenderer>();
        Rigbody = GetComponent<Rigidbody2D>();
        polCollider = GetComponent<PolygonCollider2D>();
        animator = GetComponent<Animator>();

        Control = GameObject.FindGameObjectWithTag("Controls").GetComponent<CharacterControl>();
        Parameters = Camera.main.GetComponent<Parameters>();
        AudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // Записываем стартовую позицию персонажа в респаун
        RespawnPosition = transform.position + Vector3.up;
    }

    private void FixedUpdate()
    {
        if (Life)
        {
            // Если управление не используется, анимация сбрасывается
            if (Control.Vector.x == 0) State = CharacterAnimations.Idle;

            // Если персонаж висит
            if (IsHook)
            {
                // Переключаемся на анимацию виса
                State = CharacterAnimations.Hook;
                // Если персонаж не прыгает, обнуляем скорость
                if (!IsJumping) Speed = 0;
            }

            // При нажатии на стрелки, выполняем бег
            if (Control.Vector.x < 0) Run(true);
            if (Control.Vector.x > 0) Run(false);

            // Если выполнен прыжок, проигрываем соответствующую анимацию
            if (!IsGrounded && IsJumping) State = CharacterAnimations.Jump;

            // Проверяем на касание с поверхностью
            CheckCharacterGround();

            // Если персонаж улетел за экран, уничтожаем его
            if (transform.position.y <= -15f) RecieveDamageCharacter(false, false, 0.5f);    
        }
    }

    /// <summary>Проверка нахождения персонажа на поверхности</summary>
    private void CheckCharacterGround()
    {
        // Определяем позицию коллайдера персонажа
        Point = transform.position - Offset;

        // Создаем массив коллайдеров, касающихся с персонажем
        Collider2D[] colliders = Physics2D.OverlapBoxAll(Point, Size, 0);

        // Если есть коллайдеры кроме собственного
        if (colliders.Length > 1)
        {
            // Выполняем переборку для нахождения поверхности
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.CompareTag("Surface"))
                {
                    // Восстанавливаем скорость
                    Speed = speed;
                    // Активируем нахождение на поверхности
                    IsGrounded = true;
                    // Сбрасываем прыжок
                    IsJumping = false;
                }
            }
        }
        else
        {
            // Иначе сбрасываем нахождение на поверхности
            IsGrounded = false;
            // Если персонаж висит на крюке, сбрасываем прыжок
            IsJumping = IsHook ? false : true;
        }
    }

    /// <summary>Бег персонажа (горизонтальное отображение спрайта)</summary>
    public void Run(bool flip)
    {
        // Перемещаем персонажа по вектору с указанной скоростью
        Rigbody.position = Rigbody.position + Control.Vector * Speed * Time.fixedDeltaTime;
        // Устанавливаем направление спрайта
        Sprite.flipX = flip;

        // Если персонаж на поверхности, переключаемся на анимацию бега
        if (IsGrounded) State = CharacterAnimations.Run;
    }

    /// <summary>Прыжок персонажа</summary>
    public void Jump()
    {
        // Если персонаж на поверхности или висит на крюке
        if (IsGrounded && Rigbody.velocity.y < 0.5f || IsHook)
        {
            // Создаем импульсный прыжок с указанной силой
            Rigbody.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);

            // Если персонаж висел
            if (IsHook)
            {
                // Сбрасываем вис
                IsHook = false;
                // Восстанавливаем скорость
                Speed = speed;
            }
        }
    }

    /// <summary>Вис персонажа на крюке</summary>
    public void ClingToHook()
    {
        // Отключаем гравитацию персонажа
        Rigbody.gravityScale = 0;
        // Сбрасываем физическую скорость
        Rigbody.velocity *= 0;
    }

    /// <summary>Воскрешение персонажа после проигрыша</summary>
    public void CharacterRespawn()
    {
        // Сбрасываем физическую скорость
        Rigbody.velocity *= 0;
        // Возвращаем персонажа к последнему респауну
        transform.position = RespawnPosition;

        // Восстанавливаем жизнь
        Life = true;
        // Восстанавливаем стандартную анимацию
        State = CharacterAnimations.Idle;
        // Восстанавливаем стандартный коллайдер
        polCollider.isTrigger = false;

        // Восстанавливаем слой персонажа на сцене
        Sprite.sortingOrder = layer;
    }

    /// <summary>Нанесение урона персонажу (отскок персонажа, анимация смерти, задержка до итогов уровня)</summary>
    public void RecieveDamageCharacter(bool rebound, bool animation, float time)
    {
        // Сбрасываем жизнь
        Life = false;

        // Если активен отскок
        if (rebound)
            // Отбрасываем персонажа по случайному вектору
            Rigbody.AddForce(new Vector2(Random.Range(-135.0f, 135.0f), Random.Range(160.0f, 190.0f)));

        // Если активна анимация смерти
        if (animation)
        {
            // Переключаемся на анимацию смерти
            State = CharacterAnimations.Dead;
            // Переключаем коллайдер на триггер
            polCollider.isTrigger = true;
        }

        // Отключаем кнопку паузы
        Parameters[(int)Parameters.InterfaceElements.PauseButton].SetActive(false);
        // Меняем режим игры на проигрыш
        Parameters.Mode = "lose";
        // Отображаем результат игры
        Parameters.Invoke("ShowResults", time);
    }

    /// <summary>Отображение эффекта урона</summary>
    public void ShowDamageEffect()
    {
        // Перемещаем эффект урона к персонажу
        blood.transform.position = transform.position;
        // Воспроизводим эффект
        blood.Play();
    }
}