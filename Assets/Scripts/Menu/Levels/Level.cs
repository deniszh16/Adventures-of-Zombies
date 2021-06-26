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
        
        private Image _image;
        private Button _button;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            if (_number <= PlayerPrefs.GetInt("progress"))
            {
                _image.sprite = _openLevel;
                _button.interactable = true;
            }
        }

        /// <summary>
        /// Загрузка выбранного уровня
        /// </summary>
        public void LoadLevel()
        {
            FindObjectOfType<BackgroundMusic>().SwitchMusic((int)BackgroundMusic.State.Off);

            SetsHelper.AtLevel = true;

            var transition = Camera.main.GetComponent<TransitionsController>();
            _ = StartCoroutine(transition.GoToLevel(this));
        }
    }
}