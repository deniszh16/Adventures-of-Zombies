using System;
using UnityEngine;

namespace Cubra.Controllers
{
    public class CharacterController : BaseController
    {
        // Событие смерти персонажа
        public event Action IsDead;

        // Вектор движения персонажа
        private Vector2 _direction;

        // Находится ли персонаж на земле
        private bool _isGrounded;
        // Прыгает ли персонаж
        private bool _isJumping;
        // Висит ли персонаж на крюке
        public bool IsHook { get; set; }
        // Увеличена ли скорость персонажа
        public bool IsAccelerated { get; set; }

        // Позиция для респауна персонажа
        public Vector3 RespawnPosition { get; set; }

        [Header("Управление на клавиатуре")]
        [SerializeField] private bool _keyboardControl;

        // Ссылка на персонажа
        private Character _character;

        private void Start()
        {
            _character = FindObjectOfType<Character>();

            // Записываем стартовую позицию персонажа в респаун
            RespawnPosition = _character.Transform.position + Vector3.up;

            // Подписываем отвязку камеры от персонажа
            IsDead += Main.Instance.SnapCameraToTarget;
        }

        // Управление на клавиатуре для тестирования
        #if UNITY_EDITOR || UNITY_STANDALONE
        private void Update()
        {
            if (_keyboardControl)
            {
                if (Input.GetKey("left"))
                {
                    _direction = Vector2.right * -1;
                    _character.Transform.localScale = new Vector3(-1, 1, 1);
                }
                else if (Input.GetKey("right"))
                {
                    _direction = Vector2.right;
                    _character.Transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    _direction = Vector2.zero;
                }

                if (Input.GetKeyDown("space")) ButtonJump();
            }
        }
        #endif

        private void FixedUpdate()
        {
            if (_character.Life)
            {
                // Если персонаж не движется
                if (_direction == Vector2.zero)
                {
                    _character.SetAnimation(Character.Animations.Idle);
                    if (_isJumping) _character.SetAnimation(Character.Animations.Jump);
                    if (IsHook) _character.SetAnimation(Character.Animations.Hang);
                }
                else
                {
                    // Перемещаем персонажа в указанном направлении с указанной скоростью
                    _character.Rigidbody.position = _character.Rigidbody.position + _direction * _character.Speed * Time.fixedDeltaTime;
                    if (_isGrounded) _character.SetAnimation(Character.Animations.Run);
                    if (_isJumping) _character.SetAnimation(Character.Animations.Jump);
                }

                SurfaceFinding();

                // Если персонаж улетел за экран, уничтожаем его
                if (_character.Transform.position.y < -15) DamageToCharacter(false, false);
            }
        }

        /// <summary>
        /// Нажатие на кнопку движения
        /// </summary>
        /// <param name="direction">направление движения</param>
        public void ButtonArrowDown(int direction)
        {
            if (Enabled)
            {
                // Устанавливаем вектор движения
                _direction = Vector2.right * direction;
                // Устанавливаем направление спрайта
                _character.Transform.localScale = new Vector3(direction, 1, 1);
            }
        }

        /// <summary>
        /// Отпускание кнопки движения
        /// </summary>
        public void ButtonArrowUp()
        {
            _direction = Vector2.zero;
        }

        /// <summary>
        /// Нажатие на кнопку прыжка
        /// </summary>
        public void ButtonJump()
        {
            if (Enabled)
            {
                if ((_isGrounded && _character.Rigidbody.velocity.y < 0.5f) || IsHook)
                {
                    // Создаем импульсный прыжок с указанной силой
                    _character.Rigidbody.AddForce(transform.up * _character.Jump, ForceMode2D.Impulse);

                    // Если персонаж висел, сбрасываем вис
                    if (IsHook) IsHook = false;
                }
            }
        }

        /// <summary>
        /// Поиск поверхности под персонажем
        /// </summary>
        private void SurfaceFinding()
        {
            // Определяем позицию для коллайдера
            var position = _character.Transform.position - new Vector3(0, 1.5f, 0);
            // Создаем массив поверхностей, которых касается персонаж
            Collider2D[] colliders = Physics2D.OverlapBoxAll(position, new Vector2(1, 1.5f), 0, LayerMask.GetMask("Surface"));

            if (colliders.Length > 0)
            {
                // Если персонаж ускорен и не прыгает
                if (IsAccelerated && _isJumping == false)
                {
                    IsAccelerated = false;
                    // Восстанавливаем скорость
                    _character.Speed = 8.5f;
                }

                _isGrounded = true;
                _isJumping = false;
            }
            else
            {
                _isGrounded = false;
                _isJumping = !IsHook;
            }
        }

        /// <summary>
        /// Вис персонажа на крюке
        /// </summary>
        public void HangOnHook()
        {
            // Сбрасываем гравитацию и скорость
            _character.Rigidbody.gravityScale = 0;
            _character.Rigidbody.velocity = Vector2.zero;
            _character.Speed = 0;
        }

        /// <summary>
        /// Смерть персонажа
        /// </summary>
        /// <param name="animation">анимация смерти</param>
        /// <param name="rebound">отскок персонажа</param>
        public void DamageToCharacter(bool animation, bool rebound)
        {
            if (_character.Life)
            {
                // Сбрасываем жизнь персонажа
                _character.Life = false;

                if (animation)
                {
                    // Устанавливаем анимацию смерти
                    _character.SetAnimation(Character.Animations.Dead);

                    // Устанавливаем звук смерти и воспроизводим
                    _character.SetSound(Character.Sounds.Dead);
                    _character.PlayingSound.PlaySound();

                    // Переключаем коллайдер на триггер
                    _character.PolygonCollider.isTrigger = true;
                }

                if (rebound)
                {
                    // Отбрасываем персонажа по случайному вектору
                    _character.Rigidbody.AddForce(new Vector2(UnityEngine.Random.Range(-135, -100) * _character.Transform.localScale.x, UnityEngine.Random.Range(160, 190)));
                    // Воспроизводим эффект крови
                    _character.BloodEffect.Play();
                }

                // Сообщаем подписчикам о смерти персонажа
                IsDead?.Invoke();
            }
        }

        /// <summary>
        /// Восстановление персонажа после проигрыша
        /// </summary>
        public void RestoreCharacter()
        {
            // Сбрасываем физическую скорость
            _character.Rigidbody.velocity = Vector2.zero;
            // Возвращаем персонажа к последнему респауну
            _character.Transform.position = RespawnPosition;

            // Восстанавливаем жизнь
            _character.Life = true;
            // Восстанавливаем стандартную анимацию
            _character.SetAnimation(Character.Animations.Idle);
            // Восстанавливаем стандартный коллайдер
            _character.PolygonCollider.isTrigger = false;

            // Восстанавливаем слой персонажа на сцене
            _character.SpriteRenderer.sortingOrder = 5;
        }

        /// <summary>
        /// Изменение скорости персонажа
        /// </summary>
        public void SpeedUpCharacter(float speed)
        {
            if (_character.Life)
            {
                // Если персонаж двигается, обновляем скорость
                if (_direction.x != 0) _character.Speed = speed;
            }
        }
    }
}