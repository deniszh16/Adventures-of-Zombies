using Services.PersistentProgress;
using Services.SceneLoader;
using Services.Sound;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Logic.Levels
{
    public class LevelCard : MonoBehaviour
    {
        [Header("Номер уровня")]
        [SerializeField] private int _number;
        
        [Header("Открытый уровень")]
        [SerializeField] private Sprite _openLevel;

        [Header("Компоненты кнопки")]
        [SerializeField] private Button _button;
        [SerializeField] private Image _imageButton;
        
        [Header("Сцена уровня")]
        [SerializeField] private Scenes _scene;

        [Header("Звезды за уровень")]
        [SerializeField] private Image _stars;
        [SerializeField] private Sprite[] _starSprites;

        private IPersistentProgressService _progressService;
        private ISceneLoaderService _sceneLoaderService;
        private ISoundService _soundService;

        [Inject]
        private void Construct(IPersistentProgressService progressService, ISceneLoaderService sceneLoaderService,
            ISoundService soundService)
        {
            _progressService = progressService;
            _sceneLoaderService = sceneLoaderService;
            _soundService = soundService;
        }

        private void Awake() =>
            _button.onClick.AddListener(LoadLevel);

        private void Start()
        {
            if (_number <= _progressService.GetUserProgress.Progress)
            {
                _imageButton.sprite = _openLevel;
                _button.interactable = true;

                int numberOfStars = _progressService.GetUserProgress.Stars[_number - 1];
                _stars.sprite = _starSprites[numberOfStars];
            }
        }

        private void LoadLevel()
        {
            _soundService.StopBackgroundMusic();
            _sceneLoaderService.LoadSceneAsync(_scene, screensaver: true, delay: 0f);
        }

        private void OnDestroy() =>
            _button.onClick.RemoveListener(LoadLevel);
    }
}