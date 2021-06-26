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
            if (_target == false)
            {
                _animator.SetInteger("State", (int)RatAnimations.Idle);

                _direction *= -1;
                // Определяем дистанцию (до ограничителей) для рейкаста
                _distance.x = (_direction.x > 0) ? _limiters[1].localPosition.x - transform.localPosition.x : transform.localPosition.x - _limiters[0].localPosition.x;

                RaycastHit2D hit = Physics2D.Raycast(transform.localPosition, _direction, _distance.x, LayerMask.GetMask("Character"));

                if (hit.collider == true)
                {
                    if (hit.collider.gameObject.TryGetComponent(out Character character))
                        _target = character.gameObject;
                }
            }
            else
            {
                _distance = transform.localPosition - _target.transform.position;

                if (Mathf.Abs(_distance.y) < 3.8f)
                {
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
                if (character.Life)
                {
                    _target = null;
                    _animator.SetInteger("State", (int)RatAnimations.Attack);
                    GameManager.Instance.CharacterController.DamageToCharacter(true, true);
                }
            }
        }
    }
}