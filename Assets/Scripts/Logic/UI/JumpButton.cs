using Logic.Characters;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI
{
    public class JumpButton : MonoBehaviour
    {
        [Header("Кнопка прыжка")]
        [SerializeField] private Button _button;
        
        [Header("Персонаж")]
        [SerializeField] private CharacterControl _characterControl;

        private void Awake() =>
            _button.onClick.AddListener(_characterControl.Jump);

        private void OnDestroy() =>
            _button.onClick.RemoveListener(_characterControl.Jump);
    }
}