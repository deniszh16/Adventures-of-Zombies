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
    protected enum YetiAnimation { Idle, Run, Punch, Smash, Throw, Die }

    // Установка параметра в аниматоре
    protected YetiAnimation State { set { animator.SetInteger("State", (int)value); } }

    private void Start()
    {
        triggerYeti = gameObject.transform.GetChild(0).GetComponent<CapsuleCollider2D>();

        // Определяем количество забегов
        SetQuantityRun();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // Если персонаж умер
        if (!character.Life)
            // Сбрасываем анимацию йети
            State = YetiAnimation.Idle;
    }

    public override void ActivateBoss()
    {
        // Запускаем стартовое переключение режима
        StartCoroutine(SwitchMode(0.2f, "throw"));
    }

    // Переключение активности йети
    protected override IEnumerator SwitchMode(float seconds, string mode)
    {
        yield return new WaitForSeconds(seconds);

        // Устанавливаем режим йети
        this.mode = mode;

        // Если отображаются звезды, скрываем их
        if (effect.activeSelf) effect.SetActive(false);

        switch (mode)
        {
            case "run":
                // Устанавливаем случайную скорость
                speed = Random.Range(14, 16);
                // Определяем направление
                SetDirection(true);
                // Настраиваем коллайдер
                ColliderOffset();

                // Уменьшаем количество забегов
                quantityRun--;
                // Устанавливаем анимацию бега
                State = YetiAnimation.Run;
                break;
            case "throw":
                // Определяем направление
                SetDirection(true);
                // Настраиваем коллайдер
                ColliderOffset();
                // Устанавливаем анимацию броска
                State = YetiAnimation.Throw;

                // Определяем следующий режим
                DefineNextMode();
                break;
            case "hit":
                // Определяем направление
                SetDirection(true);
                // Настраиваем коллайдер
                ColliderOffset();
                // Устанавливаем анимацию удара
                State = YetiAnimation.Punch;

                // Определяем следующий режим
                DefineNextMode();
                break;
            case "stupor":
                // Уменьшаем жизнь
                healthBoss--;
                // Создаем небольшой отскок от стены
                rigbody.AddForce(direction * -2.5f, ForceMode2D.Impulse);

                // Отображаем эффект звезд
                effect.SetActive(true);
                // Перемещаем звезды к голове быка
                effect.transform.localPosition = new Vector2(2.25f * direction.x, effect.transform.localPosition.y);

                // Сбрасываем анимацию
                State = YetiAnimation.Idle;

                // Определяем следующий режим
                DefineNextMode();
                break;
            case "attack":
                // Определяем направление
                SetDirection(true);
                // Настраиваем коллайдер
                ColliderOffset();

                // Устанавливаем анимацию удара по земле
                State = YetiAnimation.Smash;

                // Определяем количество забегов
                SetQuantityRun();
                // Определяем следующий режим
                DefineNextMode();
                break;
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
        }

        // Отображаем дополнительные препятствия
        if (mode != "attack") ShowObstacles(healthBoss);
    }

    // Определение следующего режима
    protected override void DefineNextMode()
    {
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
            // Переключаемся на режим смерти
            StartCoroutine(SwitchMode(0, "die"));
        }
    }

    // Бросок камня в персонажа
    public void ThrowStone()
    {
        // Активируем камень
        stone.SetActive(true);
        // Перемещаем камень в стартовую позицию в зависимости от направления
        stone.transform.localPosition = new Vector2(3.4f * direction.x, 0.4f);

        if (!stonePhysics)
            stonePhysics = stone.GetComponent<Rigidbody2D>();

        // Создаем импульс для броска камня
        stonePhysics.AddForce(new Vector2(Random.Range(13, 14) * direction.x, 1.5f), ForceMode2D.Impulse);

        // Через несколько секунд отключаем камень
        Invoke("DisableStone", 3.5f);
    }

    // Отключение камня
    private void DisableStone()
    {
        // Сбрасываем скорость полета
        stone.GetComponent<Rigidbody2D>().velocity *= 0;

        // Отключаем объект
        stone.SetActive(false);
    }

    // Отображение дополнительных препятствий
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

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Вызываем встряхивание камеры
            cameraShaking.ShakeCamera();
            // Переключаем режим на оглушение
            StartCoroutine(SwitchMode(0, "stupor"));
        }
    }

    // Смещение коллайдера
    private void ColliderOffset()
    {
        // Перемещаем коллайдер в зависимости от направления
        triggerYeti.offset = new Vector2((direction.x < 0) ? -2.7f : 2.7f, -0.2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если йети коснулся персонажа
        if (collision.gameObject == character.gameObject)
        {
            // Если йети не оглушен
            if (mode != "stupor")
                // Переключаем режим на удар
                StartCoroutine(SwitchMode(0, "hit"));
        }
    }

    public void HitCharacter()
    {
        // Если персонаж находится в указанном 
        if (Mathf.Abs(transform.position.x - character.transform.position.x) < 6
            && Mathf.Abs(transform.position.y - character.transform.position.y) < 2.8f)
        {
            // Наносим урон персонажу
            character.RecieveDamageCharacter(true, true, 1.6f);
            // Перемещаем эффект урона к персонажу и воспроизводим
            character.blood.transform.position = character.transform.position;
            character.blood.Play();
        } 
    }












    //protected override void Update()
    //{
    //    // Если расстояние между йети и персонажем находится в указанном диапазоне
    //    if ((Mathf.Abs(transform.position.x - character.transform.position.x) < 4.0f && Mathf.Abs(transform.position.y - character.transform.position.y) < 2.8f) && mode != "stupor")
    //        // Переключаемся на атаку кулаком
    //        StartCoroutine(SwitchMode("punch", YetiAnimation.Punch, 0));
    //}




    //protected override void OnCollisionEnter2D(Collision2D collision)
    //{
    //    base.OnCollisionEnter2D(collision);

    //    // Касание босса со стеной
    //    if (collision.gameObject.tag == "Wall")
    //        // Переключение на оглушение йети
    //        StartCoroutine(SwitchMode("stupor", YetiAnimation.Idle, 0));
    //}
}