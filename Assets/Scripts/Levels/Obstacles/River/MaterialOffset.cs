using UnityEngine;

namespace Cubra
{
    public class MaterialOffset : MonoBehaviour
    {
        [Header("Скорость движения")]
        [SerializeField] private float _offsetSpeed;

        // Вектор смещения текстуры
        private Vector3 _offset;

        // Ссылка на компонент
        private Renderer _render;

        private void Awake()
        {
            _render = GetComponent<Renderer>();
        }

        private void Update()
        {
            // Обновляем вектор смещения
            _offset.x -= _offsetSpeed * Time.deltaTime;
            // Смещаем текстуру по вектору
            _render.material.mainTextureOffset = _offset;
        }
    }
}