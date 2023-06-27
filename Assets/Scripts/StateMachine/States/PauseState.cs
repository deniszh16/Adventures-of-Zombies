using Services.Sound;
using UnityEngine;
using Zenject;

namespace StateMachine.States
{
    public class PauseState : BaseStates
    {
        [Header("Панель паузы")]
        [SerializeField] private GameObject _pausePanel;

        private ISoundService _soundService;

        [Inject]
        private void Construct(ISoundService soundService) =>
            _soundService = soundService;
        
        public override void Enter()
        {
            _pausePanel.SetActive(true);
            _soundService.PauseBackgroundMusic(true);
        }

        public override void Exit() =>
            _pausePanel.SetActive(false);
    }
}