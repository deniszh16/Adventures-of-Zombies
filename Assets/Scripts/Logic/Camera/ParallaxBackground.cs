using UnityEngine;

namespace Logic.Camera
{
    public class ParallaxBackground : MonoBehaviour
    {
        [Header("Множитель паралакса")]
        [SerializeField] private float _parallaxMultiplier;

        [Header("Компонент камеры")]
        [SerializeField] private Transform _cameraTransform;

        private Transform _transform;
        private Vector3 _lastCameraPosition;
        private float _textureUnitSizeX;

        private void Start()
        {
            _transform = transform;
            _lastCameraPosition = _cameraTransform.position;
            
            Sprite sprite = GetComponent<SpriteRenderer>().sprite;
            Texture2D texture = sprite.texture;
            _textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        }

        private void LateUpdate()
        {
            Vector3 deltaMovenent = _cameraTransform.position - _lastCameraPosition;
            
            _transform.position += new Vector3(deltaMovenent.x * _parallaxMultiplier, 0, 0);
            _lastCameraPosition = _cameraTransform.position;
            
            if (Mathf.Abs(_cameraTransform.position.x - _transform.position.x) >= _textureUnitSizeX)
            {
                float offsetPositionX = (_cameraTransform.position.x - _transform.position.x) % _textureUnitSizeX;
                _transform.position = new Vector3(_cameraTransform.position.x + offsetPositionX, _transform.position.y, _transform.position.z);
            }
        }
    }
}