using Services.GooglePlay;
using UnityEngine;
using Zenject;

namespace Logic.UI
{
    public class Authorization : MonoBehaviour
    {
        private IGooglePlayService _googlePlayService;

        [Inject]
        private void Construct(IGooglePlayService googlePlayService) =>
            _googlePlayService = googlePlayService;

        private void Start()
        {
            if (_googlePlayService.Authenticated == false)
                _googlePlayService.SignGooglePlay();
        }
    }
}