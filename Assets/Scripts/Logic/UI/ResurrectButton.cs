using Logic.Characters;
using StateMachine;
using StateMachine.States;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Logic.UI
{
    public class ResurrectButton : MonoBehaviour
    {
        [Header("Кнопка воскрешения")]
        [SerializeField] private Button _button;

        [Header("Персонаж")]
        [SerializeField] private Character _character;
        
        [Header("Компонент таймера")]
        [SerializeField] private Timer.Timer _timer;

        private GameStateMachine _gameStateMachine;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine) =>
            _gameStateMachine = gameStateMachine;

        private void Awake() =>
            _button.onClick.AddListener(ResurrectACharacter);

        private void ResurrectACharacter()
        {
            _character.CharacterRespawn();
            if (_timer.Seconds < 30) _timer.Seconds = 30;
            _gameStateMachine.Enter<PlayState>();
        }

        private void OnDestroy() =>
            _button.onClick.RemoveListener(ResurrectACharacter);
    }
}