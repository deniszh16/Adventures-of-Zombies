using Logic.Camera;
using Logic.Characters;
using Logic.UsefulObjects;
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

        [Inject]
        private void Construct(GameStateMachine gameStateMachine) =>
            _gameStateMachine = gameStateMachine;

        public override void Enter()
        {
            _characterControl.Enabled = false;
            _gameCamera.SnapCameraToTarget(_character);
            _brainsAtLevel.ResetBrainValue();
            
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