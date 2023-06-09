using Logic.Characters;
using Logic.Timer;
using UnityEngine;

namespace StateMachine.States
{
    public class PlayState : BaseStates
    {
        [Header("Компоненты персонажа")]
        [SerializeField] private CharacterControl _characterControl;
        
        [Header("Компонент таймера")]
        [SerializeField] private Timer _timer;
        
        [Header("Элементы UI")]
        [SerializeField] private GameObject _buttonPause;
        [SerializeField] private GameObject _buttonsControls;
        
        public override void Enter()
        {
            _timer.StartTimer();
            _characterControl.Enabled = true;
            _buttonPause.SetActive(true);
            _buttonsControls.SetActive(true);
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