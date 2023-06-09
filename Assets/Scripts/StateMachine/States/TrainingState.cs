using Logic.HintsAndTraining;
using UnityEngine;
using Zenject;

namespace StateMachine.States
{
    public class TrainingState : BaseStates
    {
        [Header("Компонент обучения")]
        [SerializeField] private Training _training;

        private GameStateMachine _gameStateMachine;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine) =>
            _gameStateMachine = gameStateMachine;

        public override void Enter()
        {
            _training.TrainingCompleted += GoToPlayState;
            _training.StartTraining();
        }

        public override void Exit() =>
            _training.TrainingCompleted -= GoToPlayState;

        private void GoToPlayState() =>
            _gameStateMachine.Enter<PlayState>();
    }
}