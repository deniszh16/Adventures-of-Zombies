using UnityEngine;

namespace Cubra
{
    public class Rat : MonoBehaviour
    {
        [Header("Ограничители для крысы")]
        [SerializeField] private Transform[] _limiters;

        [Header("Скорость бега")]
        [SerializeField] private float _speed;

        // Дальность обнаружения
        private Vector2 _distance;

        // Направление движения
        private Vector2 _direction;

        // Цель для крысиных атак
        private GameObject _target;

        // Перечисление анимаций крысы
        private enum RatAnimations { Idle, Run, Attack }

        private SpriteRenderer _sprite;
        private Animator _animator;

        private void Awake()
        {
            _sprite = GetComponentInChildren<SpriteRenderer>();
            _animator = GetComponent<Animator>();

            _direction = Vector2.left;
        }

        private void FixedUpdate()
        {
            // Если отсутствует цель
            if (_target == false)
            {
                _animator.SetInteger("State", (int)RatAnimations.Idle);

                _direction *= -1;
                // Определяем дистанцию (до ограничителей) для рейкаста
                _distance.x = (_direction.x > 0) ? _limiters[1].localPosition.x - transform.localPosition.x : transform.localPosition.x - _limiters[0].localPosition.x;

                RaycastHit2D hit = Physics2D.Raycast(transform.localPosition, _direction, _distance.x, LayerMask.GetMask("Character"));

                // Если найден коллайдер
                if (hit.collider == true)
                {
                    if (hit.collider.gameObject.TryGetComponent(out Character character))
                        // Записываем персонажа в цель
                        _target = character.gameObject;
                }
            }
            else
            {
                // Определяем дистанцию между крысой и целью
                _distance = transform.localPosition - _target.transform.position;

                // Если вертикальная дистанция в указанном диапазоне
                if (Mathf.Abs(_distance.y) < 3.8f)
                {
                    // Отображаем спрайт в зависимости от дистанции
                    _sprite.flipX = (_distance.x > 0) ? false : true;

                    // Если крыса не заходит за ограничители, выполняем движение
                    if (_distance.x > 0 && transform.localPosition.x > _limiters[0].localPosition.x ||
                        _distance.x < 0 && transform.localPosition.x < _limiters[1].localPosition.x) Run();
                    else
                        _animator.SetInteger("State", (int)RatAnimations.Idle);
                }
                else
                {
                    _target = null;
                }
            }
        }

        /// <summary>
        /// Бег крысы
        /// </summary>
        private void Run()
        {
            _animator.SetInteger("State", (int)RatAnimations.Run);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(_target.transform.position.x, transform.position.y), _speed * Time.deltaTime);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Character character))
            {
                // Если персонаж живой
                if (character.Life)
                {
                    _target = null;
                    _animator.SetInteger("State", (int)RatAnimations.Attack);
                    Main.Instance.CharacterController.DamageToCharacter(true, true);
                }
            }
        }
    }
}