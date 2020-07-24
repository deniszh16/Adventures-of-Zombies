using System.Collections;
using UnityEngine;

public abstract class Boss : MonoBehaviour
{
    // Режим активности
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

    // Ссылки на компоненты босса
    protected Animator animator;
    protected SpriteRenderer sprite;
    protected CapsuleCollider2D capsule;
    protected Rigidbody2D rigbody;

    // Ссылки на дополнительные компоненты
    //protected Character character;
    //protected CameraShaking cameraShaking;
   // protected CountdownToStart countdown;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        capsule = GetComponent<CapsuleCollider2D>();
        rigbody = GetComponent<Rigidbody2D>();

        //character = GameObject.FindGameObjectWithTag("Character").GetComponent<Character>();
        //cameraShaking = Camera.main.GetComponent<CameraShaking>();
        //countdown = FindObjectOfType<CountdownToStart>();
    }

    protected virtual void FixedUpdate()
    {
        // Если активен режим бега, вызываем движение
        if (mode == "run") Run();

        // Если персонаж умер
        //if (!character.Life)
        //{
        //    // Сбрасываем режим
        //    mode = "none";
        //    // Останавливаем переключение режима
        //    StopAllCoroutines();
        //}
    }

    /// <summary>Определение случайного количества забегов</summary>
    protected void SetQuantityRun()
    {
        quantityRun = Random.Range(2, 4);
    }

    /// <summary>Переключение режимов босса (время до переключения, режим активности босса)</summary>
    protected abstract IEnumerator SwitchMode(float seconds, string mode);

    /// <summary>Определение следующего режима активности босса</summary>
    protected abstract void DefineNextMode();

    /// <summary>Бег босса к персонажу</summary>
    protected void Run()
    {
        // Перемещаем босса в указанном направлении с указанной скоростью
        transform.Translate(direction * speed * Time.deltaTime);
    }

    /// <summary>Определение направления движения (горизонтальное отображение спрайта)</summary>
    protected void SetDirection(bool invert = false)
    {
        // Сравниваем позиции босса с персонажем и устанавливаем направление движения
        //direction = (transform.localPosition.x > character.transform.localPosition.x) ? Vector3.left : Vector3.right;

        if (!invert)
            // Отображаем спрайт в установленном направлении
            sprite.flipX = (direction == Vector3.left) ? true : false;
        else
            // Если установлена инверсия, разворачиваем спрайт
            sprite.flipX = (direction == Vector3.left) ? false : true;
    }

    /// <summary>Падение множества камней</summary>
    protected void Rockfall()
    {
        if (mode == "attack")
        {
            // Вызываем встряхивание камеры
            //cameraShaking.ShakeCamera();

            // Активируем набор камней
            for (int i = 0; i < stones.Length; i++) stones[i].SetActive(true);
        }
    }

    /// <summary>Отображение дополнительных препятствий</summary>
    protected abstract void ShowObstacles(int health);

    /// <summary>Отображение финишных объектов</summary>
    protected void ShowFinish()
    {
        for (int i = 0; i < objects.Length; i++)
            // Активируем объекты
            objects[i].SetActive(true);

        // Отключаем фиксацию камеры
        //Camera.main.transform.GetComponentInParent<SmoothCamera>().Limit = false;
    }
}