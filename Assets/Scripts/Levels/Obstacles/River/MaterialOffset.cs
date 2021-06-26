using UnityEngine;

namespace Cubra
{
    public class MaterialOffset : MonoBehaviour
    {
        [Header("Скорость движения")]
        [SerializeField] private float _offsetSpeed;

        // Вектор смещения текстуры
        private Vector3 _offset;
        
        private Renderer _render;

        private void Awake()
        {
            _render = GetComponent<Renderer>();
        }

        private void Update()
        {
            _offset.x -= _offsetSpeed * Time.deltaTime;
            _render.material.mainTextureOffset = _offset;
        }
    }
}