using System;
using Logic.Camera;
using Logic.Characters;
using Logic.Timer;
using Services.Sound;
using UnityEngine;
using Zenject;

namespace StateMachine.States
{
    public class PlayState : BaseStates
    {
        [Header("Компоненты персонажа")]
        [SerializeField] private Character _character;
        [SerializeField] private CharacterControl _characterControl;
        
        [Header("Игровая камера")]
        [SerializeField] private GameCamera _gameCamera;
        
        [Header("Компонент таймера")]
        [SerializeField] private Timer _timer;
        
        [Header("Элементы UI")]
        [SerializeField] private GameObject _buttonPause;
        [SerializeField] private GameObject _buttonsControls;

        public event Action LevelLaunched;

        private GameStateMachine _gameStateMachine;
        private InitialState _initialState;
        private ISoundService _soundService;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine, ISoundService soundService)
        {
            _gameStateMachine = gameStateMachine;
            _soundService = soundService;
        }

        private void Start() =>
            _initialState = _gameStateMachine.GetState<InitialState>();

        public override void Enter()
        {
            _timer.StartTimer();
            _characterControl.Enabled = true;
            
            if (_initialState.CameraBinding) 
                _gameCamera.SnapCameraToTarget(_character);
            
            _buttonPause.SetActive(true);
            _buttonsControls.SetActive(true);
            _soundService.StartBackgroundMusicOnLevels();
            
            LevelLaunched?.Invoke();
        }

        public override void Exit()
        {
            _timer.StopTimer();
            _characterControl.Enabled = false;
            _buttonPause.SetActive(false);
            _buttonsControls.SetActive(false);
        }
    }
}