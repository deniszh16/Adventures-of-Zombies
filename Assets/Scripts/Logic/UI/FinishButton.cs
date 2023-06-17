using System.Collections;
using Logic.Levels;
using StateMachine;
using StateMachine.States;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Logic.UI
{
    public class FinishButton : MonoBehaviour
    {
        private GameStateMachine _gameStateMachine;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine) =>
            _gameStateMachine = gameStateMachine;

        [Header("Компонент кнопки")]
        [SerializeField] private Button _button;
        
        [Header("Финиш уровня")]
        [SerializeField] private LevelFinish _levelFinish;

        private void Awake() =>
            _button.onClick.AddListener(FinishForLevel);

        private void FinishForLevel() =>
            StartCoroutine(FinishForLevelCoroutine());

        private IEnumerator FinishForLevelCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            _gameStateMachine.Enter<CompletedState>();
        }

        private void OnDestroy() =>
            _button.onClick.RemoveListener(FinishForLevel);
    }
}