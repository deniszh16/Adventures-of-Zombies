using Services.Achievements;
using UnityEngine;
using Zenject;

namespace Logic.UI
{
    public class Achievements : MonoBehaviour
    {
        private IAchievementsService _achievementsService;

        [Inject]
        private void Construct(IAchievementsService achievementsService) =>
            _achievementsService = achievementsService;

        private void Start() =>
            _achievementsService.RunAchievementCheck();
    }
}