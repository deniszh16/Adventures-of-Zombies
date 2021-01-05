using System.Collections;
using UnityEngine;

namespace Cubra
{
    public class Yeti : Boss
    {
        [Header("Камень для броска")]
        [SerializeField] private GameObject _stone;

        private Rigidbody2D _stonePhysics;

        // Ссылка на дочерний коллайдер
        private CapsuleCollider2D _triggerYeti;

        // Перечисление анимаций йети
        private enum YetiAnimation { Idle, Run, Punch, Smash, Throw, Die }

        // Установка параметра в аниматоре
        private YetiAnimation State { set { _animator.SetInteger("State", (int)value); } }

        protected override void Start()
        {
            base.Start();

            if (Training.PlayerTraining)
                // Добавляем в событие завершения отсчета метод пробуждения йети
                GameManager.Instance.Countdown.AfterCountdown.AddListener(AwakenBoss);
            else
                // Добавляем в событие старта уровня метод пробуждения йети
                GameManager.Instance.LevelLaunched += AwakenBoss;

            // Получаем компонент дочернего коллайдера
            _triggerYeti = gameObject.transform.GetChild(0).GetComponent<CapsuleCollider2D>();

            // Определяем количество атакующих забегов
            SetQuantityRun();
        }

        /// <summary>
        /// Пробуждение йети
        /// </summary>
        private void AwakenBoss()
        {
            // Запускаем стартовое переключение режима
            StartCoroutine(SwitchMode(0.2f, "throw"));
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            // Если персонаж умер
            if (_character.Life == false)
                State = YetiAnimation.Idle;
        }

        /// <summary>
        /// Переключение режимов босса
        /// </summary>
        /// <param name="seconds">время до переключения</param>
        /// <param name="mode">режим активности</param>
        protected override IEnumerator SwitchMode(float seconds, string mode)
        {
            yield return new WaitForSeconds(seconds);

            // Устанавливаем режим йети
            this._mode = mode;

            // Если отображаются оглушающие звезды, скрываем их
            if (_effectStars.activeSelf) _effectStars.SetActive(false);

            switch (mode)
            {
                case "run":
                    _speed = Random.Range(14, 16);

                    _quantityRuns--;
                    State = YetiAnimation.Run;
                    goto default;

                case "throw":
                    State = YetiAnimation.Throw;
                    DefineNextMode();
                    goto default;

                case "hit":
                    State = YetiAnimation.Punch;
                    DefineNextMode();
                    goto default;

                case "stupor":
                    _health--;

                    // Создаем небольшой отскок от стены
                    _rigbody.AddForce(_direction * -2.5f, ForceMode2D.Impulse);

                    _effectStars.SetActive(true);
                    // Перемещаем звезды к голове йети
                    _effectStars.transform.localPosition = new Vector2(2.25f * _direction.x, _effectStars.transform.localPosition.y);
                    
                    State = YetiAnimation.Idle;
                    DefineNextMode();
                    break;

                case "attack":
                    State = YetiAnimation.Smash;
                    
                    SetQuantityRun();
                    DefineNextMode();
                    goto default;

                case "die":
                    State = YetiAnimation.Die;
                    // Отключаем коллайдер йети
                    _capsuleCollider.enabled = false;

                    // Скрываем дополнительные препятствия
                    for (int i = 0; i < _obstacles.Length; i++)
                        _obstacles[i].SetActive(false);

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
                float pause = Random.Range(2, 3.2f);

                if (_mode == "attack")
                    // Переключаемся на бросок камня
                    StartCoroutine(SwitchMode(pause, "throw"));
                else
                    // Определяем режим в зависимости от количества забегов
                    StartCoroutine(SwitchMode(pause, (_quantityRuns > 0) ? "run" : "attack"));
            }
            else
            {
                // Иначе переключаемся на режим смерти
                StartCoroutine(SwitchMode(0, "die"));
            }
        }

        /// <summary>
        /// Бросок камня в персонажа
        /// </summary>
        public void ThrowStone()
        {
            _stone.SetActive(true);
            // Перемещаем камень в стартовую позицию в зависимости от направления
            _stone.transform.localPosition = new Vector2(3.4f * _direction.x, 0.4f);

            if (_stonePhysics == false)
                _stonePhysics = _stone.GetComponent<Rigidbody2D>();

            // Создаем импульс для броска камня
            _stonePhysics.AddForce(new Vector2(Random.Range(13, 14) * _direction.x, 1.5f), ForceMode2D.Impulse);
            // Через несколько секунд отключаем камень
            Invoke("DisableStone", 3.5f);
        }

        /// <summary>
        /// Смещение коллайдера в сторону движения йети
        /// </summary>
        private void ColliderOffset()
        {
            _triggerYeti.offset = new Vector2((_direction.x < 0) ? -2.7f : 2.7f, -0.2f);
        }

        /// <summary>
        /// Отключение камня после броска
        /// </summary>
        private void DisableStone()
        {
            _stone.GetComponent<Rigidbody2D>().velocity *= 0;
            _stone.SetActive(false);
        }

        /// <summary>
        /// Отображение дополнительных препятствий
        /// </summary>
        /// <param name="health">здоровье быка, при котором отображается препятствие</param>
        protected override void ShowObstacles(int health)
        {
            switch (health)
            {
                case 12:
                    // Отображаем пилу
                    _obstacles[0].SetActive(true);
                    break;
                case 6:
                    // Отображаем капкан
                    _obstacles[1].SetActive(true);
                    break;
            }
        }

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                // Вызываем встряхивание камеры
                StartCoroutine(_cameraShaking.ShakeCamera(0.6f, 2.1f, 1.9f));
                // Переключаем режим на оглушения
                StartCoroutine(SwitchMode(0, "stupor"));
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == _character.gameObject)
            {
               // Если йети не оглушен
               if (_mode != "stupor")
                   // Переключаем режим активности на удар
                   StartCoroutine(SwitchMode(0, "hit"));
            }
        }

        /// <summary>
        /// Удар йети по персонажу
        /// </summary>
        public void HitCharacter()
        {
            // Если персонаж находится в указанном диапазоне
            if (Mathf.Abs(transform.position.x - _character.transform.position.x) < 6
               && Mathf.Abs(transform.position.y - _character.transform.position.y) < 2.8f)
            {
               // Наносим урон персонажу
                GameManager.Instance.CharacterController.DamageToCharacter(true, true);
            } 
        }
    }
}