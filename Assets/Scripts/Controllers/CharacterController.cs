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
                if (_direction == Vector2.zero)
                {
                    _character.SetAnimation(Character.Animations.Idle);
                    if (_isJumping) _character.SetAnimation(Character.Animations.Jump);
                    if (IsHook) _character.SetAnimation(Character.Animations.Hang);
                }
                else
                {
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
                _direction = Vector2.right * direction;
                // Устанавливаем направление спрайта (через transform для поворота коллайдера)
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
            // Позиция для коллайдера персонажа
            var position = _character.Transform.position - new Vector3(0, 1.5f, 0);
            // Массив поверхностей, которых касается персонаж
            Collider2D[] colliders = Physics2D.OverlapBoxAll(position, new Vector2(1, 1.5f), 0, LayerMask.GetMask("Surface"));

            if (colliders.Length > 0)
            {
                // Если персонаж ускорен и не прыгает
                if (IsAccelerated && _isJumping == false)
                {
                    IsAccelerated = false;
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
                _character.Life = false;
                _character.Rigidbody.velocity = Vector2.zero;

                if (animation)
                {
                    _character.SetAnimation(Character.Animations.Dead);
                    _character.SetSound(Character.Sounds.Dead);
                    _character.PlayingSound.PlaySound();

                    _character.PolygonCollider.isTrigger = true;
                }

                if (rebound)
                {
                    // Отбрасываем персонажа по случайному вектору
                    _character.Rigidbody.AddForce(new Vector2(UnityEngine.Random.Range(-135, -100) * _character.Transform.localScale.x, UnityEngine.Random.Range(160, 190)));
                    _character.BloodEffect.Play();
                }

                IsDead?.Invoke();
            }
        }

        /// <summary>
        /// Восстановление персонажа после воскрешения
        /// </summary>
        public void RestoreCharacter()
        {
            _character.Rigidbody.velocity = Vector2.zero;
            _character.Transform.position = RespawnPosition;
            _character.Life = true;
            _character.SetAnimation(Character.Animations.Idle);
            _character.PolygonCollider.isTrigger = false;
            _character.SpriteRenderer.sortingOrder = 5;
        }

        /// <summary>
        /// Изменение скорости персонажа
        /// </summary>
        public void SpeedUpCharacter(float speed)
        {
            if (_character.Life)
            {
                if (_direction.x != 0) _character.Speed = speed;
            }
        }
    }
}