using Logic.UI;
using Services.PersistentProgress;
using Services.SaveLoad;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Logic.Zombies
{
    public class ZombieCard : MonoBehaviour
    {
        [Header("Номер карточки")]
        [SerializeField] private int _number;
        
        [Header("Стоимость покупки")]
        [SerializeField] private int _price;
        
        [Header("Кнопка покупки")]
        [SerializeField] private Button _button;
        
        [Header("Карточка зомби")]
        [SerializeField] private GameObject _card;
        [SerializeField] private GameObject _zombie;

        [Header("Счетчик костей")]
        [SerializeField] private NumberOfBones _numberOfBones;

        private IPersistentProgressService _progressService;
        private ISaveLoadService _saveLoadService;

        [Inject]
        private void Construct(IPersistentProgressService progressService, ISaveLoadService saveLoadService)
        {
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }

        private void Start()
        {
            if (_progressService.UserProgress.ZombiesData.Family[_number - 1])
                ShowZombie();
            else
                _button.onClick.AddListener(BuyZombie);
        }

        private void ShowZombie()
        {
            _card.SetActive(false);
            _zombie.SetActive(true);
        }

        private void BuyZombie()
        {
            if (_progressService.UserProgress.Bones >= _price)
            {
                _progressService.UserProgress.SubtractBones(_price);
                _progressService.UserProgress.ZombiesData.Family[_number - 1] = true;
                _saveLoadService.SaveProgress();
                
                ShowZombie();
            }
            else
            {
                _numberOfBones.StartFlashingAnimation();
            }
        }

        private void OnDestroy() =>
            _button.onClick.RemoveListener(BuyZombie);
    }
}