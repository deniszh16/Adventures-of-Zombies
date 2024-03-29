﻿using System.Collections;
using Logic.Camera;
using Logic.Characters;
using Logic.UsefulObjects;
using Services.PersistentProgress;
using Services.SaveLoad;
using Services.Sound;
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
        private ISoundService _soundService;

        [Inject]
        private void Construct(IPersistentProgressService progressService, ISaveLoadService saveLoadService,
            ISoundService soundService)
        {
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _soundService = soundService;
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
            _soundService.StopBackgroundMusic();
            _losePanel.SetActive(true);
            
            UpdateProgress();
        }

        private void UpdateProgress()
        {
            _progressService.GetUserProgress.Brains += BrainsAtLevel.InitialValue - _brainsAtLevel.Brains;
            _progressService.GetUserProgress.Played += 1;
            _progressService.GetUserProgress.Deaths += 1;
            _saveLoadService.SaveProgress();
        }
    }
}