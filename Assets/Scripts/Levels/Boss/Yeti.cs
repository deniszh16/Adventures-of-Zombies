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
            {
                GameManager.Instance.Countdown.AfterCountdown.AddListener(AwakenBoss);
            }
            else
            {
                GameManager.Instance.LevelLaunched += AwakenBoss;
            }

            _triggerYeti = gameObject.transform.GetChild(0).GetComponent<CapsuleCollider2D>();
            SetQuantityRun();
        }

        /// <summary>
        /// Пробуждение йети
        /// </summary>
        private void AwakenBoss()
        {
            StartCoroutine(SwitchMode(0.2f, "throw"));
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

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
            _mode = mode;

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

                    _rigbody.AddForce(_direction * -2.5f, ForceMode2D.Impulse);

                    _effectStars.SetActive(true);
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

                    _capsuleCollider.enabled = false;

                    for (int i = 0; i < _obstacles.Length; i++)
                        _obstacles[i].SetActive(false);

                    ShowFinish();
                    break;

                default:
                    SetDirection(true);
                    ColliderOffset();
                    break;
            }

            if (mode == "run")
                ShowObstacles(_health);
        }

        /// <summary>
        /// Определение следующего режима активности
        /// </summary>
        protected override void DefineNextMode()
        {
            if (_health > 0)
            {
                float pause = Random.Range(2, 3.2f);

                if (_mode == "attack")
                    StartCoroutine(SwitchMode(pause, "throw"));
                else
                    StartCoroutine(SwitchMode(pause, (_quantityRuns > 0) ? "run" : "attack"));
            }
            else
            {
                StartCoroutine(SwitchMode(0, "die"));
            }
        }

        /// <summary>
        /// Бросок камня в персонажа
        /// </summary>
        public void ThrowStone()
        {
            _stone.SetActive(true);
            _stone.transform.localPosition = new Vector2(3.4f * _direction.x, 0.4f);

            if (_stonePhysics == false)
                _stonePhysics = _stone.GetComponent<Rigidbody2D>();

            _stonePhysics.AddForce(new Vector2(Random.Range(13, 14) * _direction.x, 1.5f), ForceMode2D.Impulse);
            Invoke(nameof(DisableStone), 3.5f);
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
                    _obstacles[0].SetActive(true);
                    break;
                case 6:
                    _obstacles[1].SetActive(true);
                    break;
            }
        }

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                StartCoroutine(_cameraShaking.ShakeCamera(0.6f, 2.1f, 1.9f));
                StartCoroutine(SwitchMode(0, "stupor"));
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == _character.gameObject)
            {
               if (_mode != "stupor")
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
                GameManager.Instance.CharacterController.DamageToCharacter(true, true);
            } 
        }
    }
}