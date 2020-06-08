using UnityEngine;
using UnityEngine.UI;

namespace Cubra
{
    public class TextureOffset : MonoBehaviour
    {
        [Header("Скорость смещения")]
        [SerializeField] private float _offsetSpeed;

        // Позиция текстуры
        private float _position;

        // Ссылка на компонент
        private RawImage _rawImage;

        private void Awake()
        {
            _rawImage = GetComponent<RawImage>();
        }

        private void Update()
        {
            // Вычитаем из позиции скорость движения
            _position -= (_offsetSpeed * Time.deltaTime);

            // Если позиция текстуры превышает единицу, сбрасываем значение
            if (_position > 1.0f) _position = -1.0f;

            // Обновляем позицию текстуры
            _rawImage.uvRect = new Rect(_position, 0, 1, 1);
        }
    }
}