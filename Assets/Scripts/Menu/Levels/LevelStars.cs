using UnityEngine;
using UnityEngine.UI;
using Cubra.Helpers;

namespace Cubra
{
    public class LevelStars : MonoBehaviour
    {
        // Номер уровня
        private int _number;

        [Header("Спрайты звезд")]
        [SerializeField] private Sprite[] _sprites;

        // Объект для работы с json по звездам
        private StarsHelper StarsHelper { get; set; } = new StarsHelper();
        
        private Image _image;

        public void Awake()
        {
            _number = gameObject.GetComponentInParent<Level>().Number;
            _image = GetComponent<Image>();
        }

        private void Start()
        {
            // Преобразуем сохраненную json строку в объект
            StarsHelper = JsonUtility.FromJson<StarsHelper>(PlayerPrefs.GetString("stars-level"));

            // Если существуют звезды для текущего уровня
            if (_number <= StarsHelper.Stars.Count)
                // Устанавливаем спрайт звезд из массива
                _image.sprite = _sprites[StarsHelper.Stars[_number - 1] - 1];
        }
    }
}