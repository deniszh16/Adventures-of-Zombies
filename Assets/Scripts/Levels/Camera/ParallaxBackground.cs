using UnityEngine;

namespace Cubra
{
    public class ParallaxBackground : MonoBehaviour
    {
        [Header("Множитель паралакса")]
        [SerializeField] private float _parallaxMultiplier;

        // Текущая позиция камеры
        private Transform _cameraTransform;
        // Последняя позиция камеры
        private Vector3 _lastCameraPosition;

        // Размер текстурных блоков
        private float _textureUnitSizeX;

        private void Start()
        {
            _cameraTransform = Camera.main.GetComponent<Transform>();
            _lastCameraPosition = _cameraTransform.position;

            Sprite sprite = GetComponent<SpriteRenderer>().sprite;
            Texture2D texture = sprite.texture;
            _textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        }

        private void LateUpdate()
        {
            // Вектор между текущим и последним положением камеры
            Vector3 deltaMovenent = _cameraTransform.position - _lastCameraPosition;
            // Перемещаем камеру с множителем паралакса
            transform.position += new Vector3(deltaMovenent.x * _parallaxMultiplier, 0, 0);
            // Обновляем последнюю позицию камеры
            _lastCameraPosition = _cameraTransform.position;

            if (Mathf.Abs(_cameraTransform.position.x - transform.position.x) >= _textureUnitSizeX)
            {
                float offsetPositionX = (_cameraTransform.position.x - transform.position.x) % _textureUnitSizeX;
                transform.position = new Vector3(_cameraTransform.position.x + offsetPositionX, transform.position.y, transform.position.z);
            }
        }
    }
}