using Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace Logic.UI
{
    public class SavingIncompleteProgress : MonoBehaviour
    {
        private ISaveLoadService _saveLoadService;

        [Inject]
        private void Construct(ISaveLoadService saveLoadService) =>
            _saveLoadService = saveLoadService;

        public void SaveIncompleteProgress() =>
            _saveLoadService.SaveProgress();
    }
}