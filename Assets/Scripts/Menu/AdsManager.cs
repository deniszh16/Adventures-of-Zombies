using UnityEngine;
using AppodealAds.Unity.Api;

public class AdsManager : MonoBehaviour
{
    [Header("Ключ приложения")]
    [SerializeField] private string key;

    // Инициализация рекламы Appodeal
    private void Start() { Appodeal.initialize(key, Appodeal.INTERSTITIAL | Appodeal.REWARDED_VIDEO, true); }
}