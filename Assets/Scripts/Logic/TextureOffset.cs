using UnityEngine;
using UnityEngine.UI;

namespace Logic
{
    public class TextureOffset : MonoBehaviour
    {
        [Header("Скорость смещения")]
        [SerializeField] private float _offsetSpeed;

        private RawImage _rawImage;
        private float _position;

        private void Awake() =>
            _rawImage = GetComponent<RawImage>();
        
        private void Update()
        {
            _position -= _offsetSpeed * Time.deltaTime;

            if (_position > 1.0f) _position = -1.0f;

            _rawImage.uvRect = new Rect(_position, 0, 1, 1);
        }
    }
}