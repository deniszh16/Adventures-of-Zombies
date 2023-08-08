using Logic.UsefulObjects;
using TMPro;
using UnityEngine;

namespace Logic.UI
{
    public class UIBrainsCounter : MonoBehaviour
    {
        [Header("Компонент мозгов")]
        [SerializeField] private BrainsAtLevel _brainsAtLevel;
        
        [Header("Текст количества")]
        [SerializeField] private TextMeshProUGUI _textBrains;

        private Color _changedСolor;

        private void Awake()
        {
            _brainsAtLevel.BrainsChanged += UpdateNumberOfBrains;
            _changedСolor = new Color(0.06f, 1f, 0f, 1f);
        }

        private void UpdateNumberOfBrains()
        {
            if (_brainsAtLevel.Brains > 0)
            {
                _textBrains.text = "x" + _brainsAtLevel.Brains;
                _textBrains.color = Color.white;
            }
            else
            {
                _textBrains.text = "ok";
                _textBrains.color = _changedСolor;
            }
        }

        private void OnDestroy() =>
            _brainsAtLevel.BrainsChanged -= UpdateNumberOfBrains;
    }
}