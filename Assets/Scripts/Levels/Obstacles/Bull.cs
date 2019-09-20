using System.Collections;
using UnityEngine;

public class Bull : Boss
{
    // Высота прыжка
    private float jump = 3.5f;

    // Перечисление анимаций быка
    private enum BullAnimation { Idle, Run, Attack, Dead }

    // Установка и получение параметра в аниматоре
    private BullAnimation State { set { animator.SetInteger("State", (int)value); } }

    private void Start()
    {
        // Определяем количество забегов
        SetQuantityRun();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // Если персонаж умер
        if (!character.Life)
            // Сбрасываем анимацию быка
            State = BullAnimation.Idle;
    }

    public override void ActivateBoss()
    {
        // Запускаем стартовое переключение режима
        StartCoroutine(SwitchMode(0.2f, "run"));
    }

    // Переключение активности быка
    protected override IEnumerator SwitchMode(float seconds, string mode)
    {
        yield return new WaitForSeconds(seconds);

        // Устанавливаем режим быка
        this.mode = mode;

        // Если отображаются звезды, скрываем их
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
                // Создаем небольшой отскок от стены
                rigbody.AddForce(direction * -2.5f, ForceMode2D.Impulse);

                // Отображаем эффект звезд
                effect.SetActive(true);
                // Перемещаем звезды к голове быка
                effect.transform.localPosition = new Vector2(1.5f * direction.x, effect.transform.localPosition.y);

                // Сбрасываем анимацию
                State = BullAnimation.Idle;

                // Определяем следующий режим
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

        // Отображаем дополнительные препятствия
        if (mode != "attack") ShowObstacles(healthBoss);
    }

    // Определение следующего режима
    protected override void DefineNextMode()
    {
        if (healthBoss > 0)
        {
            // Определяем паузу перед сменой режима
            float pause = Random.Range(2.2f, 3.6f);
            // Затем переключаемся на другой режим
            StartCoroutine(SwitchMode(pause, (quantityRun > 0) ? "run" : "attack"));
        }
        else
        {
            // Переключаемся на режим смерти
            StartCoroutine(SwitchMode(0, "die"));
        }
    }

    // Отображение дополнительных препятствий
    protected override void ShowObstacles(int health)
    {
        switch (health)
        {
            // Если здоровье равно десяти
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

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // Если бык коснулся персонажа
        if (collision.gameObject == character.gameObject)
        {
            // Переключаем режим на удар
            StartCoroutine(SwitchMode(0, "hit"));

            // Наносим урон персонажу
            character.RecieveDamageCharacter(true, true, 1.6f);
            // Перемещаем эффект урона к персонажу и воспроизводим
            character.blood.transform.position = character.transform.position;
            character.blood.Play();
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