using System.Collections;
using Logic.Camera;
using Logic.Characters;
using Logic.UsefulObjects;
using Services.PersistentProgress;
using Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace StateMachine.States
{
    public class LosingState : BaseStates
    {
        [Header("Панель проигрыша")]
        [SerializeField] private GameObject _losePanel;

        [Header("Компонент персонажа")]
        [SerializeField] private Character _character;
        
        [Header("Игровая камера")]
        [SerializeField] private GameCamera _gameCamera;

        [Header("Компонент мозгов")]
        [SerializeField] private BrainsAtLevel _brainsAtLevel;
        
        private IPersistentProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        [Inject]
        private void Construct(IPersistentProgressService progressService, ISaveLoadService saveLoadService)
        {
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }
        
        public override void Enter()
        {
            _gameCamera.SnapCameraToTarget(_character);
            StartCoroutine(ShowLosingPanel());
        }

        public override void Exit() =>
            _losePanel.SetActive(false);

        private IEnumerator ShowLosingPanel()
        {
            yield return new WaitForSeconds(1.5f);
            _losePanel.SetActive(true);
            UpdateProgress();
        }

        private void UpdateProgress()
        {
            _progressService.UserProgress.Brains += BrainsAtLevel.InitialValue - _brainsAtLevel.Brains;
            _progressService.UserProgress.Played += 1;
            _progressService.UserProgress.Deaths += 1;
            _saveLoadService.SaveProgress();
        }
    }
}