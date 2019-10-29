using System.Collections;
using UnityEngine;

public class Bull : Boss
{
    // Высота прыжка
    private float jump = 3.5f;

    // Перечисление анимаций быка
    private enum BullAnimation { Idle, Run, Attack, Dead }

    // Установка параметра в аниматоре
    private BullAnimation State { set { animator.SetInteger("State", (int)value); } }

    private void Start()
    {
        // Добавляем в событие завершения отсчета метод пробуждения быка
        countdown.AfterCountdown.AddListener(AwakenBoss);

        // Определяем количество атакующих забегов
        SetQuantityRun();
    }

    /// <summary>Пробуждение быка</summary>
    private void AwakenBoss()
    {
        // Запускаем стартовое переключение режима
        StartCoroutine(SwitchMode(0.2f, "run"));
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // Если персонаж умер
        if (!character.Life)
            // Сбрасываем анимацию быка
            State = BullAnimation.Idle;
    }

    /// <summary>Переключение режимов активности быка (время до переключения, режим активности босса)</summary>
    protected override IEnumerator SwitchMode(float seconds, string mode)
    {
        yield return new WaitForSeconds(seconds);

        // Устанавливаем режим быка
        this.mode = mode;

        // Если отображаются оглушающие звезды, скрываем их
        if (effect.activeSelf) effect.SetActive(false);

        switch (mode)
        {
            case "run":
                // Устанавливаем случайную скорость
                speed = Random.Range(14, 18);
                // Определяем направление
                SetDirection();

                // Уменьшаем количество забегов
                quantityRun--;

                // Устанавливаем анимацию бега
                State = BullAnimation.Run;
                break;

            case "hit":
                // Устанавливаем анимацию атаки
                State = BullAnimation.Attack;
                break;

            case "stupor":
                // Уменьшаем жизнь
                healthBoss--;

                // Создаем небольшой отскок быка от стены
                rigbody.AddForce(direction * -3.5f, ForceMode2D.Impulse);

                // Отображаем оглушающие звезды
                effect.SetActive(true);
                // Перемещаем оглушающие звезды к голове быка
                effect.transform.localPosition = new Vector2(3.2f * direction.x, effect.transform.localPosition.y);

                // Устанавливаем стандартную анимацию
                State = BullAnimation.Idle;

                // Определяем следующий режим активности
                DefineNextMode();
                break;

            case "attack":
                // Определяем направление быка
                SetDirection();

                // Переключаемся на анимацию атаки
                State = BullAnimation.Attack;

                // Определяем количество забегов
                SetQuantityRun();

                // Определяем следующий режим
                DefineNextMode();
                break;

            case "die":
                // Переключаемся на анимацию смерти
                State = BullAnimation.Dead;

                // Отключаем коллайдер быка
                capsule.enabled = false;

                // Скрываем дополнительные препятствия
                for (int i = 0; i < obstacles.Length; i++)
                    obstacles[i].SetActive(false);

                // Отображаем финишные объекты
                ShowFinish();
                break;
        }
        
        if (mode == "run")
            // Отображаем дополнительные препятствия
            ShowObstacles(healthBoss);
    }

    /// <summary>Определение следующего режима активности</summary>
    protected override void DefineNextMode()
    {
        // Если здоровье быка больше нуля
        if (healthBoss > 0)
        {
            // Определяем паузу перед сменой режима
            float pause = Random.Range(2.2f, 3.6f);

            // Затем переключаемся на другой режим
            StartCoroutine(SwitchMode(pause, (quantityRun > 0) ? "run" : "attack"));
        }
        else
        {
            // Иначе переключаемся на режим смерти
            StartCoroutine(SwitchMode(0, "die"));
        }
    }

    /// <summary>Отображение дополнительных препятствий (здоровье быка, при котором отображается препятствие)</summary>
    protected override void ShowObstacles(int health)
    {
        switch (health)
        {
            case 10:
                // Отображаем шипы
                obstacles[0].SetActive(true);
                break;
            case 5:
                // Отображаем стрелы
                obstacles[1].SetActive(true);
                // Активируем создание стрел
                obstacles[1].GetComponent<SpawnObject>().StartCoroutine("CreateObject");
                break;
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        // Если бык коснулся персонажа
        if (collision.gameObject == character.gameObject)
        {
            // Переключаем режим активности на удар
            StartCoroutine(SwitchMode(0, "hit"));

            // Отображаем эффект урона
            character.ShowDamageEffect();
            // Наносим урон персонажу
            character.RecieveDamageCharacter(true, true, 1.6f);
        }
        // Если бык врезался в стену
        else if (collision.gameObject.CompareTag("Wall"))
        {
            // Вызываем встряхивание камеры
            cameraShaking.ShakeCamera();

            // Переключаем режим на оглушение
            StartCoroutine(SwitchMode(0, "stupor"));
        }
    }
}