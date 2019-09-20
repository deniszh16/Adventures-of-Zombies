using UnityEngine;

public class Character : MonoBehaviour
{
    // Жизнь персонажа
    public bool Life { get; set; } = true;

    // Скорость движения персонажа
    public float Speed { get; set; } = 7.5f;

    // Высота прыжка персонажа
    private float JumpForce { get; } = 12f;

    // Проверка на прыжок персонажа
    public bool IsJumping { get; set; } = false;

    // Проверка нахождения на земле
    private bool IsGrounded { get; set; } = false;

    // Позиция и размер коллайдера персонажа
    private Vector2 Point { get; set; } = Vector2.zero;
    private Vector2 Size { get; } = new Vector2(0.5f, 0.75f);

    // Проверка на вис и зацеп за крюк
    public bool IsHook { get; set; } = false;
    public bool CatchHook { get; set; } = false;

    // Перечисление анимаций персонажа
    private enum CharacterAnimations { Idle, Run, Jump, Dead, Hook }

    // Переключение анимаций персонажа
    private CharacterAnimations State
    {
        // Установка параметра в аниматоре
        set { animator.SetInteger("State", (int)value); }
    }

    // Позиция респауна персонажа
    public Vector3 RespawnPosition { get; set; }

    [Header("Эффект урона")] public ParticleSystem blood;

    // Ссылки на используемые компоненты
    public SpriteRenderer Sprite { get; set; }
    public Rigidbody2D Rigbody { get; set; }
    public AudioSource AudioSource { get; set; }
    private PolygonCollider2D polCollider;
    private Animator animator;
    

    // Ссылки на управление и параметры
    public CharacterControl Control { get; set; }
    public Parameters Parameters { get; set; }

    private void Awake()
    {
        Sprite = GetComponentInChildren<SpriteRenderer>();
        polCollider = GetComponent<PolygonCollider2D>();
        Rigbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        AudioSource = GetComponent<AudioSource>();

        Control = GameObject.FindGameObjectWithTag("Controls").GetComponent<CharacterControl>();
        Parameters = Camera.main.GetComponent<Parameters>();
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

            // Если персонаж висит на крюке
            if (IsHook)
            {
                // Переключаемся на анимацию виса
                State = CharacterAnimations.Hook;
                // Если персонаж не прыгает, сбрасываем скорость
                if (!IsJumping) Speed = 0;
            }

            // Если нажата кнопка влево, выполняется бег влево
            if (Control.Vector.x < 0) Run(true);
            // Если нажата кнопка вправо, выполняется бег вправо
            if (Control.Vector.x > 0) Run(false);

            // Если персонаж прыгает, проигрывается анимация прыжка
            if (!IsGrounded && IsJumping) State = CharacterAnimations.Jump;

            // Проверяем персонажа на касание с поверхностью
            CheckCharacterGround();

            // Если персонаж улетел за экран, уничтожаем его
            if (transform.position.y <= -15f) RecieveDamageCharacter(false, false, 0.5f);    
        }
    }

    // Проверка нахождения персонажа на поверхности
    private void CheckCharacterGround()
    {
        // Определяем позицию коллайдера персонажа
        Point = transform.position - new Vector3(0.1f, 1.0f, 0);

        // Создаем массив коллайдеров, касающихся с персонажем
        Collider2D[] colliders = Physics2D.OverlapBoxAll(Point, Size, 0);

        // Если коллайдеров больше одного
        if (colliders.Length > 1)
        {
            // Выполняем переборку коллайдеров для нахождения поверхности
            foreach (Collider2D collider in colliders)
            {
                // Если поверхность найдена
                if (collider.gameObject.CompareTag("Surface"))
                {
                    // Сбрасываем скорость
                    Speed = 7.5f;
                    // Активируем переменную поверхности
                    IsGrounded = true;
                    // Сбрасываем переменную прыжка
                    IsJumping = false;
                }
            }
        }
        else
        {
            // Сбрасываем переменную поверхности
            IsGrounded = false;
            // Если персонаж висит на крюке, сбрасываем прыжок
            IsJumping = (IsHook) ? false : true;
        }
    }

    // Бег персонажа
    public void Run(bool flip)
    {
        // Перемещаем персонажа по вектору с указанной скоростью
        Rigbody.position = Rigbody.position + Control.Vector * Speed * Time.fixedDeltaTime;
        // Указываем напрвление спрайта
        Sprite.flipX = flip;

        // Если персонаж на поверхности, переключаемся на анимацию бега
        if (IsGrounded) State = CharacterAnimations.Run;
    }

    // Прыжок персонажа
    public void Jump()
    {
        // Если персонаж на поверхности или висит на крюке
        if (IsGrounded && Rigbody.velocity.y < 0.5f || IsHook)
        {
            // Создаем импульсный прыжок с указанной силой
            Rigbody.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
            // Сбрасываем переменную виса
            IsHook = false;

            // Если скорость нулевая, восстанавливаем стандартную
            if (Speed <= 0) Speed = 7.5f;
        }
    }

    // Вис персонажа на крюке
    public void ClingToHook()
    {
        // Отключаем гравитацию персонажа
        Rigbody.gravityScale = 0;
        // Сбрасываем физическую скорость
        Rigbody.velocity *= 0;

        // Сбрасываем переменную зацепа
        CatchHook = false;
    }

    // Воскрешение персонажа после проигрыша
    public void CharacterRespawn()
    {
        // Сбрасываем физическую скорость
        Rigbody.velocity *= 0;
        // Возвращаем персонажа к последнему респауну
        transform.position = RespawnPosition;

        // Активируем жизнь
        Life = true;
        // Восстанавливаем стандартную анимацию
        State = CharacterAnimations.Idle;

        // Восстанавливаем стандартный коллайдер
        polCollider.isTrigger = false;

        // Восстанавливаем слой персонажа
        Sprite.sortingOrder = 5;
    }

    // Нанесение урона персонажу
    public void RecieveDamageCharacter(bool rebound, bool animation, float time)
    {
        // Сбрасываем жизнь
        Life = false;

        // Скрываем кнопку паузы
        Parameters.interfaceElements[(int)Parameters.InterfaceElements.PauseButton].SetActive(false);

        // Если активен отскок, отбрасываем персонажа по случайному вектору
        if (rebound) Rigbody.AddForce(new Vector2(Random.Range(-135.0f, 135.0f), Random.Range(160.0f, 190.0f)));

        // Если активна анимация смерти
        if (animation)
        {
            // Переключаемся на анимацию смерти
            State = CharacterAnimations.Dead;
            // Переключаем коллайдер на триггер
            polCollider.isTrigger = true;
        }

        // Меняем режим игры на проигрыш
        Parameters.Mode = "lose";
        // Отображаем результат игры
        Parameters.Invoke("ShowResults", time);
    }
}