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

        private ISoundService _soundService;

        [Inject]
        private void Construct(ISoundService soundService) =>
            _soundService = soundService;

        public override void Enter()
        {
            _timer.StartTimer();
            _characterControl.Enabled = true;
            _gameCamera.SnapCameraToTarget(_character);
            _buttonPause.SetActive(true);
            _buttonsControls.SetActive(true);
            _soundService.StartBackgroundMusicOnLevels();
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