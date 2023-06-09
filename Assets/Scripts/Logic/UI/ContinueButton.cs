using StateMachine;
using StateMachine.States;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Logic.UI
{
    public class ContinueButton : MonoBehaviour
    {
        [Header("Компонент кнопки")]
        [SerializeField] private Button _button;
        
        private GameStateMachine _gameStateMachine;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine) =>
            _gameStateMachine = gameStateMachine;
        
        private void Awake() =>
            _button.onClick.AddListener(ContinueGame);

        private void ContinueGame() =>
            _gameStateMachine.Enter<PlayState>();

        private void OnDestroy() =>
            _button.onClick.RemoveListener(ContinueGame);
    }
}