using System.Collections;
using UnityEngine;

namespace Cubra
{
    public class Bull : Boss
    {
        // Перечисление анимаций быка
        private enum BullAnimation { Idle, Run, Attack, Dead }

        // Установка параметра в аниматоре
        private BullAnimation State { set { _animator.SetInteger("State", (int)value); } }

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

            SetQuantityRun();
        }

        /// <summary>
        /// Пробуждение быка
        /// </summary>
        private void AwakenBoss()
        {
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
            _mode = mode;

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

                    _rigbody.AddForce(_direction * -3.5f, ForceMode2D.Impulse);

                    _effectStars.SetActive(true);
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

                    _capsuleCollider.enabled = false;

                    for (int i = 0; i < _obstacles.Length; i++)
                        _obstacles[i].SetActive(false);

                    ShowFinish();
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
                float pause = Random.Range(2.2f, 3.6f);
                StartCoroutine(SwitchMode(pause, (_quantityRuns > 0) ? "run" : "attack"));
            }
            else
            {
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
                    _obstacles[0].SetActive(true);
                    break;
                case 5:
                    _obstacles[1].SetActive(true);
                    _obstacles[1].GetComponent<SpawnObject>().StartCoroutine("CreateObject");
                    break;
            }
        }

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject == _character.gameObject)
            {
                StartCoroutine(SwitchMode(0, "hit"));
                GameManager.Instance.CharacterController.DamageToCharacter(true, true);
            }
            else if (collision.gameObject.CompareTag("Wall"))
            {
                StartCoroutine(_cameraShaking.ShakeCamera(0.6f, 2.1f, 1.9f));
                StartCoroutine(SwitchMode(0, "stupor"));
            }
        }
    }
}