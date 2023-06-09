using UnityEngine;
using UnityEngine.UI;

namespace Logic.HintsAndTraining
{
    public class HintUpdateButton : MonoBehaviour
    {
        [Header("Компонент подсказок")]
        [SerializeField] private Training _training;
        
        [Header("Компонент кнопки")]
        [SerializeField] private Button _button;

        private void Awake() =>
            _button.onClick.AddListener(UpdateHint);

        private void UpdateHint()
        {
            if (_training.CheckNumberOfHints())
                _training.UpdateTrainingText();
            else
                _training.OnTrainingCompleted();
        }

        private void OnDestroy() =>
            _button.onClick.RemoveListener(UpdateHint);
    }
}