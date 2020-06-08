using UnityEngine;

namespace Cubra
{
    public class Achievements : MonoBehaviour
    {
        private void Start()
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                // Если пройден первый уровень, открываем достижение (сложное начало)
                if (PlayerPrefs.GetInt("progress") > 1) GooglePlayServices.UnlockingAchievement(GPGSIds.achievement);

                // Если собраны 20 мозгов на уровнях, открываем достижение (вкуснятина)
                if (PlayerPrefs.GetInt("brains") >= 20) GooglePlayServices.UnlockingAchievement(GPGSIds.achievement_3);

                // Если пройден 7 уровень с боссом, открываем достижение (коварный бык)
                if (PlayerPrefs.GetInt("progress") > 7) GooglePlayServices.UnlockingAchievement(GPGSIds.achievement_9);

                // Если уничтожено 25 бочек, открываем достижение (красные бочки)
                if (PlayerPrefs.GetInt("barrel") >= 25) GooglePlayServices.UnlockingAchievement(GPGSIds.achievement_11);

                // Если собрано 35 мозгов, открываем достижение (любимая еда)
                if (PlayerPrefs.GetInt("brains") >= 35) GooglePlayServices.UnlockingAchievement(GPGSIds.achievement_12);

                // Если пройден 15 уровень с боссом, открываем достижение (грозный йети)
                if (PlayerPrefs.GetInt("progress") > 15) GooglePlayServices.UnlockingAchievement(GPGSIds.achievement_14);

                // Если общее количество собранных монет более тысячи, открываем достижение (много золота)
                if (PlayerPrefs.GetInt("piggy-bank") >= 1000) GooglePlayServices.UnlockingAchievement(GPGSIds.achievement_15);
            }
        }
    }
}