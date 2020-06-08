using UnityEngine;
using AppodealAds.Unity.Api;

namespace Cubra
{
    public class AdsManager : MonoBehaviour
    {
        [Header("Ключ приложения")]
        [SerializeField] private string _key;

        private void Start()
        {
            Appodeal.initialize(_key, Appodeal.INTERSTITIAL | Appodeal.REWARDED_VIDEO, true);
        }
    }
}