using UnityEngine;

namespace Logic
{
    public class MaterialOffset : MonoBehaviour
    {
        [Header("Скорость движения")]
        [SerializeField] private float _offsetSpeed;
        
        [Header("Компонент рендера")]
        [SerializeField] private Renderer _render;
        
        private Vector2 _offset;

        private void Update()
        {
            _offset.x -= _offsetSpeed * Time.deltaTime;
            _render.material.mainTextureOffset = _offset;
        }
    }
}