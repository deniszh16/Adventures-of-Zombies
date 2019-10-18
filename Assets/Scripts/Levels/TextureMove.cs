using UnityEngine;

public class TextureMove : MonoBehaviour
{
    [Header("Скорость движения")]
    [SerializeField] private float speed = 0.15f;

    // Вектор смещения текстуры
    private Vector2 offset;

    // Ссылка на компонент рендера
    private Renderer render;

    private void Awake()
    {
        render = GetComponent<Renderer>();
    }

    private void Update()
    {
        // Обновляем вектор
        offset.x -= speed * Time.deltaTime;
        // Смещаем текстуру по вектору
        render.material.mainTextureOffset = offset;
    }
}