using System.Collections;
using System.Linq;
using Logic.UI;
using Services.PersistentProgress;
using Services.SaveLoad;
using UnityEngine;
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

        [Inject]
        private void Construct(IPersistentProgressService persistentProgress, ISaveLoadService saveLoadService)
        {
            _persistentProgress = persistentProgress;
            _saveLoadService = saveLoadService;
        }

        public bool CheckAchievementCompletion(int number) =>
            _persistentProgress.UserProgress.AchievementsData.Statuses[number - 1];

        public void RunAchievementCheck()
        {
            if (CheckAchievementCompletion(number: 1) != true && _persistentProgress.UserProgress.Progress > 1)
                UnlockAchievement(number: 1);
            
            if (CheckAchievementCompletion(number: 2) != true && _persistentProgress.UserProgress.Stars.Contains(3))
                UnlockAchievement(number: 2);
            
            if (CheckAchievementCompletion(number: 3) != true && _persistentProgress.UserProgress.Brains >= 15)
                UnlockAchievement(number: 3);
            
            if (CheckAchievementCompletion(number: 4) != true && _persistentProgress.UserProgress.Resurrection)
                UnlockAchievement(number: 4);
            
            if (CheckAchievementCompletion(number: 5) != true && _persistentProgress.UserProgress.DestroyedBarrel >= 20)
                UnlockAchievement(number: 5);
            
            if (CheckAchievementCompletion(number: 6) != true && _persistentProgress.UserProgress.Progress >= 6)
                UnlockAchievement(number: 6);
            
            if (CheckAchievementCompletion(number: 7) != true && _persistentProgress.UserProgress.Brains >= 30)
                UnlockAchievement(number: 7);
            
            if (CheckAchievementCompletion(number: 8) != true && _persistentProgress.UserProgress.CheckAllStars())
                UnlockAchievement(number: 8);
            
            if (CheckAchievementCompletion(number: 9) != true && _persistentProgress.UserProgress.Progress >= 10)
                UnlockAchievement(number: 9);
            
            if (CheckAchievementCompletion(number: 10) != true && _persistentProgress.UserProgress.Bones >= 350)
                UnlockAchievement(number: 10);
            
            if (CheckAchievementCompletion(number: 11) != true && _persistentProgress.UserProgress.ZombiesData.CheckAllCharacters())
                UnlockAchievement(number: 11);
        }

        private void UnlockAchievement(int number)
        {
            if (_persistentProgress.UserProgress.AchievementsData.Statuses[number - 1] != true)
            {
                _persistentProgress.UserProgress.AchievementsData.Statuses[number - 1] = true;
                ShowAchievementBar(number);
                
                _persistentProgress.UserProgress.AchievementsData.Statuses[number] = true;
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