using Services.Sound;
using UnityEngine;
using Zenject;

namespace StateMachine.States
{
    public class PauseState : BaseStates
    {
        [Header("Панель паузы")]
        [SerializeField] private GameObject _pausePanel;
        
        [Header("Остановка времени")]
        [SerializeField] private bool _stopTime;

        private ISoundService _soundService;

        [Inject]
        private void Construct(ISoundService soundService) =>
            _soundService = soundService;
        
        public override void Enter()
        {
            _pausePanel.SetActive(true);
            _soundService.PauseBackgroundMusic(true);
            if (_stopTime) Time.timeScale = 0;
        }

        public override void Exit()
        {
            _pausePanel.SetActive(false);
            if (_stopTime) Time.timeScale = 1;
        }
    }
}