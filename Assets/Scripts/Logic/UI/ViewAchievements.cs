using Services.GooglePlay;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Logic.UI
{
    public class ViewAchievements : MonoBehaviour
    {
        [Header("Кнопка просмотра")]
        [SerializeField] private Button _button;
        
        private IGooglePlayService _googlePlayService;

        [Inject]
        private void Construct(IGooglePlayService googlePlayService) =>
            _googlePlayService = googlePlayService;

        private void Awake() =>
            _button.onClick.AddListener(ShowAchievements);

        private void ShowAchievements() =>
            _googlePlayService.ShowAchievements();

        private void OnDestroy() =>
            _button.onClick.RemoveListener(ShowAchievements);
    }
}