using System.Collections;
using UnityEngine;

public abstract class Boss : MonoBehaviour
{
    // Тип активности
    protected string mode;

    // Количество атакующих забегов
    protected int quantityRun;

    [Header("Здоровье босса")]
    [SerializeField] protected int healthBoss;

    // Скорость бега
    protected float speed;

    // Направление движения
    protected Vector3 direction = Vector3.left;

    [Header("Эффект звезд")]
    [SerializeField] protected GameObject effect;

    [Header("Падающие камни")]
    [SerializeField] protected GameObject[] stones;

    [Header("Препятствия на уровне")]
    [SerializeField] protected GameObject[] obstacles;

    [Header("Финишные объекты")]
    [SerializeField] protected GameObject[] objects;

    // Ссылки на компоненты
    protected Animator animator;
    protected SpriteRenderer sprite;
    protected CapsuleCollider2D capsule;
    protected Rigidbody2D rigbody;

    protected Character character;
    protected CameraShaking cameraShaking;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        capsule = GetComponent<CapsuleCollider2D>();
        rigbody = GetComponent<Rigidbody2D>();

        character = GameObject.FindGameObjectWithTag("Character").GetComponent<Character>();
        cameraShaking = Camera.main.GetComponent<CameraShaking>();
    }

    protected virtual void FixedUpdate()
    {
        // Если активен режим бега, вызываем движение
        if (mode == "run") Run();

        // Если персонаж умер
        if (!character.Life)
        {
            // Сбрасываем режим
            mode = "none";
            // Останавливаем все переключения
            StopAllCoroutines();
        }
    }

    // Случайное количество забегов
    protected void SetQuantityRun()
    {
        // Определяем количество забегов
        quantityRun = Random.Range(2, 4);
    }

    // Активация босса на уровне
    public abstract void ActivateBoss();

    // Переключение режимов босса
    protected abstract IEnumerator SwitchMode(float seconds, string mode);

    // Определение следующего режима босса
    protected abstract void DefineNextMode();

    // Бег босса
    protected void Run()
    {
        // Перемещаем босса в указанном направлении с указанной скоростью
        transform.Translate(direction * speed * Time.deltaTime);
    }

    // Установка направления босса
    protected void SetDirection(bool invert = false)
    {
        // Сравниваем позиции босса с персонажем и устанавливаем направление движения
        direction = (transform.localPosition.x > character.transform.localPosition.x) ? Vector3.left : Vector3.right;

        if (!invert)
            // Отображаем спрайт в установленном направлении
            sprite.flipX = (direction == Vector3.left) ? true : false;
        else
            // Если установлена инверсия, разворачиваем спрайт
            sprite.flipX = (direction == Vector3.left) ? false : true;
    }

    // Столкновения на уровне
    protected abstract void OnCollisionEnter2D(Collision2D collision);

    // Падение множества камней
    protected void Rockfall()
    {
        // Если активен режим атаки
        if (mode == "attack")
        {
            // Вызываем встряхивание камеры
            cameraShaking.ShakeCamera();

            // Активируем все камни
            for (int i = 0; i < stones.Length; i++) stones[i].SetActive(true);
        }
    }

    // Отображение дополнительных препятствий
    protected abstract void ShowObstacles(int health);

    // Отображение финишных объектов
    protected void ShowFinish()
    {
        for (int i = 0; i < objects.Length; i++)
            objects[i].SetActive(true);

        // Активируем перемещение камеры
        Camera.main.transform.GetComponentInParent<SmoothCamera>().limit = false;
    }
}