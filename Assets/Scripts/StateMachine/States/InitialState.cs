using Logic.Camera;
using Logic.Characters;
using Logic.UsefulObjects;
using Services.Sound;
using UnityEngine;
using Zenject;

namespace StateMachine.States
{
    public class InitialState : BaseStates
    {
        [Header("Компоненты персонажа")]
        [SerializeField] private Character _character;
        [SerializeField] private CharacterControl _characterControl;
        
        [Header("Игровая камера")]
        [SerializeField] private GameCamera _gameCamera;
        
        [Header("Компонент мозгов")]
        [SerializeField] private BrainsAtLevel _brainsAtLevel;

        private GameStateMachine _gameStateMachine;
        private ISoundService _soundService;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine, ISoundService soundService)
        {
            _gameStateMachine = gameStateMachine;
            _soundService = soundService;
        }

        public override void Enter()
        {
            _characterControl.Enabled = false;
            _character.RespawnPosition = _character.transform.position;
            _gameCamera.SnapCameraToTarget(_character);
            _brainsAtLevel.ResetBrainValue();
            _soundService.PrepareBackgroundMusicOnLevel();
            
            if (_gameStateMachine.CheckState(typeof(TrainingState)))
                _gameStateMachine.Enter<TrainingState>();
            else
                _gameStateMachine.Enter<PlayState>();
        }

        public override void Exit()
        {
        }
    }
}