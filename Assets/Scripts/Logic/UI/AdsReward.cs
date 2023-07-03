﻿using Services.Ads;
using Services.PersistentProgress;
using Services.SaveLoad;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Logic.UI
{
    public class AdsReward : MonoBehaviour
    {
        [Header("Кнопка бонуса")]
        [SerializeField] private Button _button;

        private IPersistentProgressService _progressService;
        private ISaveLoadService _saveLoadService;
        private IAdService _adService;

        [Inject]
        private void Construct(IPersistentProgressService progressService, ISaveLoadService saveLoadService,
            IAdService adService)
        {
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _adService = adService;
        }

        private void Awake()
        {
            _button.onClick.AddListener(_adService.ShowRewardedAd);
            _adService.RewardedVideoFinished += AccrueBonus;
        }

        private void AccrueBonus()
        {
            _progressService.UserProgress.ChangeBones(100);
            _saveLoadService.SaveProgress();
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(_adService.ShowRewardedAd);
            _adService.RewardedVideoFinished -= AccrueBonus;
        }
    }
}