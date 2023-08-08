using Services.Input;
using UnityEngine;
using Zenject;

namespace Logic.Characters
{
    public class CharacterControl : MonoBehaviour
    {
        [Header("Компоненты персонажа")]
        [SerializeField] private Character _character;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Animator _animator;
        private Transform _transform;

        [Header("Высота прыжка")]
        [SerializeField] private float _jumpHeight;
        [Header("Скорость движения")]
        [SerializeField] private float _speed;
        [Header("Ускорение прыжка")]
        [SerializeField] private float _jumpMultiplier;
        [Header("Скорость падения")]
        [SerializeField] private float _fallMultiplier;

        private const float StandardSpeed = 10f;
        private const float StandardGravity = 1f;

        public bool Enabled { get; set; }

        private bool _isGrounded;
        private bool _isJumping;
        
        public bool IsHanging { get; set; }
        public bool IsAccelerated { get; set; }

        private float _direction;
        private Vector3 _vectorLeft;
        private int _layerMask;

        private static readonly int RunningAnimation = Animator.StringToHash("Run");
        private static readonly int JumpAnimation = Animator.StringToHash("Jump");
        private static readonly int HangingAnimation = Animator.StringToHash("Hanging");

        private IInputService _inputService;

        [Inject]
        private void Construct(IInputService inputService) =>
            _inputService = inputService;

        private void Awake()
        {
            _layerMask = 1 << LayerMask.NameToLayer("Surface");
            _vectorLeft = new Vector3(-1f, 1f, 1f);
            _transform = transform;
        }

        private void Update()
        {
            if (Enabled != true || _character.Life != true)
            {
                _direction = 0;
                return;
            }

            _direction = _inputService.HorizontalAxis;
            SetDirection();
            
            if (_inputService.IsJumpButtonUp)
                Jump();
        }

        private void FixedUpdate()
        {
            Run();
            SpeedUpJump();
            FindSurface();
        }

        private void SetDirection()
        {
            if (_direction == 0)
                return;

            _transform.localScale = _direction > 0 ? Vector3.one : _vectorLeft;
        }

        public void ChangeDirectionVector()
        {
            if (_direction > 0.2f) _direction = 0.2f;
            else if (_direction < -0.2f) _direction = -0.2f;
        }

        private void Run()
        {
            _rigidbody.velocity = new Vector2(_direction * _speed, _rigidbody.velocity.y);
            _animator.SetBool(RunningAnimation, _direction != 0);
        }

        public void Jump()
        {
            if (_isGrounded && _isJumping != true && _rigidbody.velocity.y < 5f || IsHanging)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0f);
                _rigidbody.AddForce(new Vector2(0f, _jumpHeight), ForceMode2D.Impulse);
                _animator.SetTrigger(JumpAnimation);
                if (IsHanging) IsHanging = false;
                _isJumping = true;
            }
        }

        private void SpeedUpJump()
        {
            if (_rigidbody.velocity.y < 0)
            {
                _rigidbody.velocity +=
                    Vector2.up * (Physics2D.gravity.y * (_fallMultiplier - 1) * Time.fixedDeltaTime);
            }
            else if (_rigidbody.velocity.y > 0)
            {
                _rigidbody.velocity +=
                    Vector2.up * (Physics2D.gravity.y * (_jumpMultiplier - 1) * Time.fixedDeltaTime);
            }
        }

        private void FindSurface()
        {
            Vector2 position = _transform.position - new Vector3(0.2f, 1.9f, 0);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.5f, _layerMask);

            if (colliders.Length > 0)
            {
                _isGrounded = true;
                _isJumping = false;
                
                if (IsAccelerated == false && _isGrounded && _isJumping == false)
                    RestoreSpeed();
            }
            else
            {
                _isGrounded = false;
            }
        }

        public void SpeedUpCharacter(float speed)
        {
            if (_character.Life)
            {
                IsAccelerated = true;
                if (_direction != 0) _speed = speed;
            }
        }
        
        public void HangOnHook() 
        {
            _rigidbody.gravityScale = 0; 
            _rigidbody.velocity = Vector2.zero;
            _speed = 0;
        }

        public void HangOnHookAnimation() =>
            _animator.SetTrigger(HangingAnimation);

        public void RestoreSpeed() =>
            _speed = StandardSpeed;

        public void RestoreGravity() =>
            _rigidbody.gravityScale = StandardGravity;
    }
}