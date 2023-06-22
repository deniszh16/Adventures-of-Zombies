using System.Collections;
using UnityEngine;
using Zenject;
using TMPro;

namespace StateMachine.States
{
    public class CountdownState : BaseStates
    {
        [Header("Текст отсчета")]
        [SerializeField] private TextMeshProUGUI _textComponent;
        
        private int _countdown = 3;
        
        private GameStateMachine _gameStateMachine;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine) =>
            _gameStateMachine = gameStateMachine;

        public override void Enter()
        {
            _countdown = 3;
            _textComponent.gameObject.SetActive(true);
            _ = StartCoroutine(StartCountdown());
        }
        
        private IEnumerator StartCountdown()
        {
            WaitForSeconds second = new WaitForSeconds(1);

            while (_countdown > 0)
            {
                _textComponent.text = _countdown.ToString();
                yield return second;
                _countdown--;
            }
            
            _gameStateMachine.Enter<PlayState>();
        }

        public override void Exit() =>
            _textComponent.gameObject.SetActive(false);
    }
}