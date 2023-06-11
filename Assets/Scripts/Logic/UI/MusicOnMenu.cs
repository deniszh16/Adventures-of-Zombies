using Services.Sound;
using UnityEngine;
using Zenject;

namespace Logic.UI
{
    public class MusicOnMenu : MonoBehaviour
    {
        private ISoundService _soundService;

        [Inject]
        private void Construct(ISoundService soundService) =>
            _soundService = soundService;

        private void Start() =>
            _soundService.StartBackgroundMusicInMenu();
    }
}