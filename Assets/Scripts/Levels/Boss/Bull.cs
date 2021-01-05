using System.Collections;
using UnityEngine;

namespace Cubra
{
    public class Bull : Boss
    {
        // Высота прыжка
        private float _jump = 3.5f;

        // Перечисление анимаций быка
        private enum BullAnimation { Idle, Run, Attack, Dead }

        // Установка параметра в аниматоре
        private BullAnimation State { set { _animator.SetInteger("State", (int)value); } }

        protected override void Start()
        {
            base.Start();

            if (Training.PlayerTraining)
                // Добавляем в событие завершения отсчета метод пробуждения быка
                GameManager.Instance.Countdown.AfterCountdown.AddListener(AwakenBoss);
            else
                // Добавляем в событие старта уровня метод пробуждения быка
                GameManager.Instance.LevelLaunched += AwakenBoss;

            // Определяем количество забегов
            SetQuantityRun();
        }

        /// <summary>
        /// Пробуждение быка
        /// </summary>
        private void AwakenBoss()
        {
            // Запускаем стартовое переключение режима
            StartCoroutine(SwitchMode(0.3f, "run"));
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (_character.Life == false)
                State = BullAnimation.Idle;
        }

        /// <summary>
        /// Переключение режимов босса
        /// </summary>
        /// <param name="seconds">время до переключения</param>
        /// <param name="mode">режим активности</param>
        protected override IEnumerator SwitchMode(float seconds, string mode)
        {
            yield return new WaitForSeconds(seconds);

            // Устанавливаем режим быка
            this._mode = mode;

            // Если отображаются оглушающие звезды, скрываем их
            if (_effectStars.activeSelf) _effectStars.SetActive(false);

            switch (mode)
            {
                case "run":
                    _speed = Random.Range(14, 18);
                    SetDirection();

                    _quantityRuns--;
                    State = BullAnimation.Run;
                    break;

                case "hit":
                    State = BullAnimation.Attack;
                    break;

                case "stupor":
                    _health--;

                    // Создаем небольшой отскок быка от стены
                    _rigbody.AddForce(_direction * -3.5f, ForceMode2D.Impulse);

                    _effectStars.SetActive(true);
                    // Перемещаем оглушающие звезды к голове быка
                    _effectStars.transform.localPosition = new Vector2(3.2f * _direction.x, _effectStars.transform.localPosition.y);

                    State = BullAnimation.Idle;
                    DefineNextMode();
                    break;

                case "attack":
                    SetDirection();
                    State = BullAnimation.Attack;

                    SetQuantityRun();
                    DefineNextMode();
                    break;

                case "die":
                    State = BullAnimation.Dead;
                    // Отключаем коллайдер быка
                    _capsuleCollider.enabled = false;

                    // Скрываем дополнительные препятствия
                    for (int i = 0; i < _obstacles.Length; i++)
                        _obstacles[i].SetActive(false);

                    ShowFinish();
                    break;
            }

            if (mode == "run")
                // Отображаем дополнительные препятствия
                ShowObstacles(_health);
        }

        /// <summary>
        /// Определение следующего режима активности
        /// </summary>
        protected override void DefineNextMode()
        {
            if (_health > 0)
            {
                // Определяем паузу перед сменой режима
                float pause = Random.Range(2.2f, 3.6f);
                // Затем переключаемся на другой режим
                StartCoroutine(SwitchMode(pause, (_quantityRuns > 0) ? "run" : "attack"));
            }
            else
            {
                // Иначе переключаемся на режим смерти
                StartCoroutine(SwitchMode(0, "die"));
            }
        }

        /// <summary>
        /// Отображение дополнительных препятствий
        /// </summary>
        /// <param name="health">здоровье быка, при котором отображается препятствие</param>
        protected override void ShowObstacles(int health)
        {
            switch (health)
            {
                case 10:
                    // Отображаем шипы
                    _obstacles[0].SetActive(true);
                    break;
                case 5:
                    // Отображаем стрелы
                    _obstacles[1].SetActive(true);
                    // Активируем создание стрел
                    _obstacles[1].GetComponent<SpawnObject>().StartCoroutine("CreateObject");
                    break;
            }
        }

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject == _character.gameObject)
            {
                // Переключаем режим активности на удар
                StartCoroutine(SwitchMode(0, "hit"));
                // Наносим урон персонажу
                GameManager.Instance.CharacterController.DamageToCharacter(true, true);
            }
            else if (collision.gameObject.CompareTag("Wall"))
            {
                // Вызываем встряхивание камеры
                StartCoroutine(_cameraShaking.ShakeCamera(0.6f, 2.1f, 1.9f));
                StartCoroutine(SwitchMode(0, "stupor"));
            }
        }
    }
}