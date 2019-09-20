using UnityEngine;

public class TextureMove : MonoBehaviour
{
    [Header("Скорость движения")]
    [SerializeField] private float speed = 0.15f;

    // Вектор смещения текстуры
    private Vector2 offset;

    private Renderer render;

    private void Awake() { render = GetComponent<Renderer>(); }

    private void Update()
    {
        // Обновление вектора
        offset.x -= speed * Time.deltaTime;
        // Смешение текстуры по вектору
        render.material.mainTextureOffset = offset;
    }
}