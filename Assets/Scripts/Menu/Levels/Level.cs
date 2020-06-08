using UnityEngine;
using UnityEngine.UI;
using Cubra.Controllers;

namespace Cubra
{
    public class Level : MonoBehaviour
    {
        [Header("Номер уровня")]
        [SerializeField] private int _number;

        public int Number => _number;

        [Header("Открытый уровень")]
        [SerializeField] private Sprite _openLevel;

        // Ссылки на компоненты
        private Image _image;
        private Button _button;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            // Если номер уровня меньше прогресса
            if (_number <= PlayerPrefs.GetInt("progress"))
            {
                // Отображаем открытую карточку
                _image.sprite = _openLevel;
                // Активируем кнопку
                _button.interactable = true;
            }
        }

        /// <summary>
        /// Загрузка выбранного уровня
        /// </summary>
        public void LoadLevel()
        {
            // Отключаем фоновую музыку в меню
            FindObjectOfType<BackgroundMusic>().SwitchMusic((int)BackgroundMusic.State.Off);

            // Активируем нахождение на уровне
            ChoiceSets.AtLevel = true;

            var transition = Camera.main.GetComponent<TransitionsController>();
            // Выполняем асинхронную загрузку уровня
            _ = StartCoroutine(transition.GoToLevel(this));
        }
    }
}