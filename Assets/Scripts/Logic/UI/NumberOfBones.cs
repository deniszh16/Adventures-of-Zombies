using Services.PersistentProgress;
using UnityEngine;
using Zenject;
using TMPro;

namespace Logic.UI
{
    public class NumberOfBones : MonoBehaviour
    {
        [Header("Компоненты счетчика")]
        [SerializeField] private TextMeshProUGUI _textComponent;
        [SerializeField] private Animator _animator;
        
        private static readonly int FlashingAnimation = Animator.StringToHash("Flashing");

        private IPersistentProgressService _progressService;

        [Inject]
        private void Construct(IPersistentProgressService progressService) =>
            _progressService = progressService;

        private void Awake() =>
            _progressService.GetUserProgress.BonesChanged += GetNumberOfBones;

        private void Start() =>
            GetNumberOfBones();

        private void GetNumberOfBones() =>
            _textComponent.text = _progressService.GetUserProgress.Bones.ToString();

        public void StartFlashingAnimation() =>
            _animator.SetTrigger(FlashingAnimation);

        private void OnDestroy() =>
            _progressService.GetUserProgress.BonesChanged -= GetNumberOfBones;
    }
}