using Services.Achievements;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Logic.UI
{
    public class Achievement : MonoBehaviour
    {
        [Header("Компоненты карточки")]
        [SerializeField] private int _number;
        
        [Header("Компоненты карточки")]
        [SerializeField] private Image _achievementIcon;
        [SerializeField] private GameObject _icon;

        private IAchievementsService _achievementsService;

        [Inject]
        private void Construct(IAchievementsService achievementsService) =>
            _achievementsService = achievementsService;

        private void Start()
        {
            if (_achievementsService.CheckAchievementCompletion(_number))
            {
                _achievementIcon.color = Color.white;
                _icon.SetActive(true);
            }
        }
    }
}