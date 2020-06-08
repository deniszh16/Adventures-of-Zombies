using UnityEngine;
using UnityEngine.UI;

namespace Cubra
{
    public class ZombieSelection : MonoBehaviour
    {
        [Header("Кнопка выбора")]
        [SerializeField] private Button _selectButton;

        // Номер персонажа
        private int _number;

        // Ссылки на компоненты
        private Text _textButton;
        private TextTranslation _textTranslation;

        private void Awake()
        {
            _number = gameObject.GetComponent<Zombie>().Number;
            _textButton = _selectButton.GetComponentInChildren<Text>();
            _textTranslation = _textButton.gameObject.GetComponent<TextTranslation>();
        }

        /// <summary>
        /// Настройка кнопки выбора персонажа
        /// </summary>
        /// <param name="active">доступность персонажа для выбора</param>
        public void ChangeButton(bool active)
        {
            if (active)
            {
                ChangeButton();
            }
            else
            {
                // Отключаем кнопку выбора
                _selectButton.interactable = false;
                // Обновляем перевод текста
                _textTranslation.TranslateText();
            }
        }

        /// <summary>
        /// Настройка кнопки выбора персонажа
        /// </summary>
        public void ChangeButton()
        {
            // Если текущий персонаж выбран
            if (_number == PlayerPrefs.GetInt("character"))
            {
                // Отключаем кнопку выбора
                _selectButton.interactable = false;
                // Обновляем текст на кнопке
                _textTranslation.ChangeKey("button-selected");
            }
            else
            {
                // Активируем кнопку выбора
                _selectButton.interactable = true;
                // Обновляем текст на кнопке
                _textTranslation.ChangeKey("button-select");
            }
        }
    }
}