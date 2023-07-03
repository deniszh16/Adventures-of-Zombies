using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI
{
    public class ExitingApplication : MonoBehaviour
    {
        [Header("Кнопка выхода")]
        [SerializeField] private Button _button;

        private void Awake() =>
            _button.onClick.AddListener(ExitApp);

        private void ExitApp() =>
            Application.Quit();

        private void OnDestroy() =>
            _button.onClick.RemoveListener(ExitApp);
    }
}