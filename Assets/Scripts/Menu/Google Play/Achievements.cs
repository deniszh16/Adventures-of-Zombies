using UnityEngine;

namespace Cubra
{
    public class Achievements : MonoBehaviour
    {
        private void Start()
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                if (PlayerPrefs.GetInt("progress") > 1)
                    GooglePlayServices.UnlockingAchievement(GPGSIds.achievement);

                if (PlayerPrefs.GetInt("brains") >= 20)
                    GooglePlayServices.UnlockingAchievement(GPGSIds.achievement_3);

                if (PlayerPrefs.GetInt("progress") > 7)
                    GooglePlayServices.UnlockingAchievement(GPGSIds.achievement_9);

                if (PlayerPrefs.GetInt("barrel") >= 25)
                    GooglePlayServices.UnlockingAchievement(GPGSIds.achievement_11);

                if (PlayerPrefs.GetInt("brains") >= 35)
                    GooglePlayServices.UnlockingAchievement(GPGSIds.achievement_12);

                if (PlayerPrefs.GetInt("progress") > 15)
                    GooglePlayServices.UnlockingAchievement(GPGSIds.achievement_14);

                if (PlayerPrefs.GetInt("piggy-bank") >= 1000)
                    GooglePlayServices.UnlockingAchievement(GPGSIds.achievement_15);
            }
        }
    }
}