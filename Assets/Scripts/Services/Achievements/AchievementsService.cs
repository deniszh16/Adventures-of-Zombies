using Services.PersistentProgress;
using Services.GooglePlay;
using System.Collections;
using Services.SaveLoad;
using System.Linq;
using UnityEngine;
using Logic.UI;
using Zenject;

namespace Services.Achievements
{
    public class AchievementsService : MonoBehaviour, IAchievementsService
    {
        [Header("Плашка достижения")]
        [SerializeField] private Animator _animator;
        
        [Header("Перевод текста")]
        [SerializeField] private TextTranslation _textTranslation;

        private static readonly int DepartureAnimation = Animator.StringToHash("Appearance");
        private static readonly int HideAnimation = Animator.StringToHash("Hiding");
        
        private IPersistentProgressService _persistentProgress;
        private ISaveLoadService _saveLoadService;
        private IGooglePlayService _googlePlayService;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgress, ISaveLoadService saveLoadService,
            IGooglePlayService googlePlayService)
        {
            _persistentProgress = persistentProgress;
            _saveLoadService = saveLoadService;
            _googlePlayService = googlePlayService;
        }

        public bool CheckAchievementCompletion(int number) =>
            _persistentProgress.GetUserProgress.AchievementsData.Statuses[number - 1];

        public void RunAchievementCheck()
        {
            if (CheckAchievementCompletion(number: 1) != true && _persistentProgress.GetUserProgress.Progress > 1)
            {
                UnlockAchievement(number: 1);
                _googlePlayService.UnlockAchievement(GPGSIds.achievement);
            }

            if (CheckAchievementCompletion(number: 2) != true && _persistentProgress.GetUserProgress.Stars.Contains(3))
            {
                UnlockAchievement(number: 2); 
                _googlePlayService.UnlockAchievement(GPGSIds.achievement_2);
            }

            if (CheckAchievementCompletion(number: 3) != true && _persistentProgress.GetUserProgress.Brains >= 15)
            {
                UnlockAchievement(number: 3);
                _googlePlayService.UnlockAchievement(GPGSIds.achievement_3);
            }

            if (CheckAchievementCompletion(number: 4) != true && _persistentProgress.GetUserProgress.Resurrection)
            {
                UnlockAchievement(number: 4);
                _googlePlayService.UnlockAchievement(GPGSIds.achievement_10);
            }

            if (CheckAchievementCompletion(number: 5) != true && _persistentProgress.GetUserProgress.DestroyedBarrel >= 20)
            {
                UnlockAchievement(number: 5);
                _googlePlayService.UnlockAchievement(GPGSIds.achievement_11);
            }

            if (CheckAchievementCompletion(number: 6) != true && _persistentProgress.GetUserProgress.Progress >= 6)
            {
                UnlockAchievement(number: 6);
                _googlePlayService.UnlockAchievement(GPGSIds.achievement_9);
            }

            if (CheckAchievementCompletion(number: 7) != true && _persistentProgress.GetUserProgress.Brains >= 30)
            {
                UnlockAchievement(number: 7);
                _googlePlayService.UnlockAchievement(GPGSIds.achievement_12);
            }

            if (CheckAchievementCompletion(number: 8) != true && _persistentProgress.GetUserProgress.CheckAllStars())
            {
                UnlockAchievement(number: 8);
                _googlePlayService.UnlockAchievement(GPGSIds.achievement_12);
            }

            if (CheckAchievementCompletion(number: 9) != true && _persistentProgress.GetUserProgress.Progress >= 10)
            {
                UnlockAchievement(number: 9);
                _googlePlayService.UnlockAchievement(GPGSIds.achievement_14);
            }

            if (CheckAchievementCompletion(number: 10) != true && _persistentProgress.GetUserProgress.Bones >= 350)
            {
                UnlockAchievement(number: 10);
            }

            if (CheckAchievementCompletion(number: 11) != true &&
                _persistentProgress.GetUserProgress.ZombiesData.CheckAllCharacters())
            {
                UnlockAchievement(number: 11);
                _googlePlayService.UnlockAchievement(GPGSIds.achievement_15);
            }
        }

        private void UnlockAchievement(int number)
        {
            if (_persistentProgress.GetUserProgress.AchievementsData.Statuses[number - 1] != true)
            {
                _persistentProgress.GetUserProgress.AchievementsData.Statuses[number - 1] = true;
                ShowAchievementBar(number);
                
                _persistentProgress.GetUserProgress.AchievementsData.Statuses[number - 1] = true;
                _saveLoadService.SaveProgress();
            }
        }

        private void ShowAchievementBar(int number)
        {
            _textTranslation.ChangeKey("achievement-title-" + number);
            _animator.gameObject.SetActive(true);
            _animator.Play(DepartureAnimation);
            
            _ = StartCoroutine(HideAchievementBar());
        }

        private IEnumerator HideAchievementBar()
        {
            yield return new WaitForSeconds(2f);
            _animator.Play(HideAnimation);
            yield return new WaitForSeconds(1f);
            _animator.gameObject.SetActive(false);
        }
    }
}