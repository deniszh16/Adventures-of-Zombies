using System.Collections;
using UnityEngine;

public class Yeti : Boss
{
    [Header("Камень для броска")]
    [SerializeField] private GameObject stone;

    // Ссылка на физику камня
    private Rigidbody2D stonePhysics;

    // Ссылка на дочерний коллайдер
    private CapsuleCollider2D triggerYeti;

    // Перечисление анимаций йети
    private enum YetiAnimation { Idle, Run, Punch, Smash, Throw, Die }

    // Установка параметра в аниматоре
    private YetiAnimation State { set { animator.SetInteger("State", (int)value); } }

    private void Start()
    {
        // Добавляем в событие завершения отсчета метод пробуждения йети
        countdown.AfterCountdown.AddListener(AwakenBoss);

        // Получаем компонент дочернего коллайдера
        triggerYeti = gameObject.transform.GetChild(0).GetComponent<CapsuleCollider2D>();

        // Определяем количество атакующих забегов
        SetQuantityRun();
    }

    /// <summary>Пробуждение йети</summary>
    private void AwakenBoss()
    {
        // Запускаем стартовое переключение режима
        StartCoroutine(SwitchMode(0.2f, "throw"));
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // Если персонаж умер
        if (!character.Life)
            // Сбрасываем анимацию йети
            State = YetiAnimation.Idle;
    }

    /// <summary>Определение следующего режима активности йети</summary>
    protected override IEnumerator SwitchMode(float seconds, string mode)
    {
        yield return new WaitForSeconds(seconds);

        // Устанавливаем режим йети
        this.mode = mode;

        // Если отображаются оглушающие звезды, скрываем их
        if (effect.activeSelf) effect.SetActive(false);

        switch (mode)
        {
            case "run":
                // Устанавливаем случайную скорость
                speed = Random.Range(14, 16);

                // Уменьшаем количество забегов
                quantityRun--;

                // Устанавливаем анимацию бега
                State = YetiAnimation.Run;
                goto default;

            case "throw":
                // Устанавливаем анимацию броска
                State = YetiAnimation.Throw;

                // Определяем следующий режим
                DefineNextMode();
                goto default;

            case "hit":
                // Устанавливаем анимацию удара
                State = YetiAnimation.Punch;

                // Определяем следующий режим
                DefineNextMode();
                goto default;

            case "stupor":
                // Уменьшаем жизнь
                healthBoss--;

                // Создаем небольшой отскок от стены
                rigbody.AddForce(direction * -2.5f, ForceMode2D.Impulse);

                // Отображаем эффект звезд
                effect.SetActive(true);
                // Перемещаем звезды к голове йети
                effect.transform.localPosition = new Vector2(2.25f * direction.x, effect.transform.localPosition.y);

                // Сбрасываем анимацию
                State = YetiAnimation.Idle;

                // Определяем следующий режим
                DefineNextMode();
                break;

            case "attack":
                // Устанавливаем анимацию удара по земле
                State = YetiAnimation.Smash;

                // Определяем количество забегов
                SetQuantityRun();

                // Определяем следующий режим
                DefineNextMode();
                goto default;

            case "die":
                // Переключаемся на анимацию смерти
                State = YetiAnimation.Die;
                // Отключаем коллайдер йети
                capsule.enabled = false;

                // Скрываем дополнительные препятствия
                for (int i = 0; i < obstacles.Length; i++)
                    obstacles[i].SetActive(false);

                // Отображаем финишные объекты
                ShowFinish();
                break;

            default:
                // Определяем направление
                SetDirection(true);
                // Смещаем коллайдер касания в направлении движения
                ColliderOffset();
                break;
        }
        
        if (mode == "run")
            // Отображаем дополнительные препятствия
            ShowObstacles(healthBoss);
    }

    /// <summary>Определение следующего режима активности</summary>
    protected override void DefineNextMode()
    {
        // Если здоровье йети больше нуля
        if (healthBoss > 0)
        {
            // Определяем паузу перед сменой режима
            float pause = Random.Range(2, 3.2f);

            if (mode == "attack")
                // Переключаемся на бросок камня
                StartCoroutine(SwitchMode(pause, "throw"));
            else
                // Определяем режим в зависимости от количества забегов
                StartCoroutine(SwitchMode(pause, (quantityRun > 0) ? "run" : "attack"));
        }
        else
        {
            // Иначе переключаемся на режим смерти
            StartCoroutine(SwitchMode(0, "die"));
        }
    }

    /// <summary>Бросок камня в персонажа</summary>
    public void ThrowStone()
    {
        // Активируем камень
        stone.SetActive(true);
        // Перемещаем камень в стартовую позицию в зависимости от направления
        stone.transform.localPosition = new Vector2(3.4f * direction.x, 0.4f);

        if (!stonePhysics)
            // Получаем физический компонент у камня
            stonePhysics = stone.GetComponent<Rigidbody2D>();

        // Создаем импульс для броска камня
        stonePhysics.AddForce(new Vector2(Random.Range(13, 14) * direction.x, 1.5f), ForceMode2D.Impulse);

        // Через несколько секунд отключаем камень
        Invoke("DisableStone", 3.5f);
    }

    /// <summary>Смещение коллайдера в сторону движения йети</summary>
    private void ColliderOffset()
    {
        // Перемещаем коллайдер в зависимости от направления
        triggerYeti.offset = new Vector2((direction.x < 0) ? -2.7f : 2.7f, -0.2f);
    }

    /// <summary>Отключение камня после броска</summary>
    private void DisableStone()
    {
        // Сбрасываем скорость полета
        stone.GetComponent<Rigidbody2D>().velocity *= 0;

        // Отключаем объект
        stone.SetActive(false);
    }

    /// <summary>Отображение дополнительных препятствий (здоровье йети, при котором отображается препятствие)</summary>
    protected override void ShowObstacles(int health)
    {
        switch (health)
        {
            case 12:
                // Отображаем пилу
                obstacles[0].SetActive(true);
                break;
            case 6:
                // Отображаем капкан
                obstacles[1].SetActive(true);
                break;
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        // Если йети врезался в стену
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Вызываем встряхивание камеры
            cameraShaking.ShakeCamera();
            // Переключаем режим на оглушения
            StartCoroutine(SwitchMode(0, "stupor"));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если йети коснулся персонажа
        if (collision.gameObject == character.gameObject)
        {
            // Если йети не оглушен
            if (mode != "stupor")
                // Переключаем режим активности на удар
                StartCoroutine(SwitchMode(0, "hit"));
        }
    }

    /// <summary>Удар йети по персонажу</summary>
    public void HitCharacter()
    {
        // Если персонаж находится в указанном диапазоне
        if (Mathf.Abs(transform.position.x - character.transform.position.x) < 6
            && Mathf.Abs(transform.position.y - character.transform.position.y) < 2.8f)
        {
            // Наносим урон персонажу
            character.RecieveDamageCharacter(true, true, 1.6f);

            // Перемещаем эффект урона к персонажу и воспроизводим
            character.Blood.transform.position = character.transform.position;
            character.Blood.Play();
        } 
    }
}