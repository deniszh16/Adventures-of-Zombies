using Logic.Characters;
using UnityEngine;

namespace Logic.Obstacles
{
    public class Rat : MonoBehaviour
    {
        [Header("Компоненты крысы")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Animator _animator;
        private Transform _transform;
        
        [Header("Ограничители бега")]
        [SerializeField] private Transform[] _limiters;

        [Header("Скорость бега")]
        [SerializeField] private float _speed;
        
        private Vector2 _distance;
        private Vector2 _direction;
        private GameObject _target;
        private int _layerMask;
        
        private static readonly int RunAnimation = Animator.StringToHash("Run");
        private static readonly int AttackAnimation = Animator.StringToHash("Attack");

        private void Awake()
        {
            _transform = transform;
            _layerMask = 1 << LayerMask.NameToLayer("Character");
            _direction = Vector2.left;
        }

        private void Update()
        {
            if (_target)
            {
                _distance = _transform.localPosition - _target.transform.position;
                
                if (Mathf.Abs(_distance.y) < 3.8f)
                {
                    _spriteRenderer.flipX = !(_distance.x > 0);

                    if (_distance.x > 0 && _transform.localPosition.x > _limiters[1].localPosition.x ||
                        _distance.x < 0 && _transform.localPosition.x < _limiters[0].localPosition.x)
                    {
                        Run();
                    }
                    else
                    {
                        _animator.SetBool(RunAnimation, false);
                    }
                }
                else
                {
                    _target = null;
                }
            }
        }

        private void FixedUpdate()
        {
            if (_target == false)
            {
                _animator.SetBool(RunAnimation, false);
                
                _direction *= -1;
                _distance.x = _direction.x > 0 ?
                    _limiters[1].localPosition.x - _transform.localPosition.x
                    : _transform.localPosition.x - _limiters[0].localPosition.x;
                
                RaycastHit2D hit = Physics2D.Raycast(_transform.localPosition, _direction, _distance.x, _layerMask);
                
                if (hit.collider)
                    _target = hit.collider.gameObject;
            }
        }
        
        private void Run()
        {
            _animator.SetBool(RunAnimation, true);
            _transform.position = Vector2.MoveTowards(_transform.position, 
                new Vector2(_target.transform.position.x, _transform.position.y), _speed * Time.deltaTime);
        }
        
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Character character)) 
            {
                if (character.Life) 
                {
                    _target = null;
                    _animator.SetTrigger(AttackAnimation);
                    character.DamageToCharacter();
                    character.ShowDeathAnimation();
                }
            }
        }
    }
}