using Logic.Camera;
using Logic.Characters;
using StateMachine;
using StateMachine.States;
using UnityEngine;
using Zenject;

namespace Logic.Bosses
{
    public abstract class Boss : MonoBehaviour
    {
        [Header("Стейт машина")]
        [SerializeField] protected BossStateMachine _bossStateMachine;

        [Header("Здоровье босса")]
        [SerializeField] protected int _health;

        public int Health => _health;

        [Header("Компоненты босса")]
        [SerializeField] protected SpriteRenderer _spriteRenderer;

        [Header("Прочие компоненты")]
        [SerializeField] protected Character _character;
        [SerializeField] protected GameCamera _gameCamera;
        private Transform _transform;
        
        public int QuantityRuns { get; set; }
        public Vector3 Direction { get; private set; }
        public bool RunningActivity { get; set; }
        
        private float _speed;

        private GameStateMachine _gameStateMachine;
        private PlayState _playState;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine) =>
            _gameStateMachine = gameStateMachine;

        private void Start()
        {
            _transform = transform;
            Direction = Vector3.left;
            
            _playState = _gameStateMachine.GetState<PlayState>();
            _playState.LevelLaunched += GoToStartState;
            _character.CharacterDied += GoToIdleState;
        }

        protected abstract void GoToStartState();

        public void SetQuantityRun(int min, int max) =>
            QuantityRuns = Random.Range(min, max);

        public void SetBossSpeed(float min, float max) =>
            _speed = Random.Range(min, max);
        
        public void SetDirection(bool spriteReversal)
        {
            Direction = transform.localPosition.x > _character.transform.localPosition.x ? Vector3.left : Vector3.right;

            if (spriteReversal == false)
                _spriteRenderer.flipX = Direction == Vector3.left;
            else
                _spriteRenderer.flipX = Direction != Vector3.left;
        }

        private void Run() =>
            _transform.Translate(Direction * (_speed * Time.deltaTime));

        public void DecreaseHealth(int value) =>
            _health -= value;

        private void Update()
        {
            if (RunningActivity)
                Run();
        }

        public abstract void DefineNextState();

        protected abstract void GoToIdleState();

        public void SnapCameraToCharacter() =>
            _gameCamera.SnapCameraToTarget(_character);

        private void OnDestroy()
        {
            _playState.LevelLaunched -= GoToStartState;
            _character.CharacterDied -= GoToIdleState;
        }
    }
}