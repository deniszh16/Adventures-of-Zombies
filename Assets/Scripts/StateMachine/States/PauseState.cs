using UnityEngine;

namespace StateMachine.States
{
    public class PauseState : BaseStates
    {
        [Header("Панель паузы")]
        [SerializeField] private GameObject _pausePanel;
        
        public override void Enter() =>
            _pausePanel.SetActive(true);

        public override void Exit() =>
            _pausePanel.SetActive(false);
    }
}