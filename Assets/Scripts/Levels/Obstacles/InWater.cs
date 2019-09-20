using UnityEngine;

public class InWater : MonoBehaviour
{
    [Header("Масса в воде")]
    public float mass;

    [Header("Отображение эффекта")]
    public bool spray = false;

    [Header("Проигрывание звука")]
    public bool playingSound = true;

    // Ссылки на компоненты
    public SpriteRenderer Sprite { get; set; }
    public Rigidbody2D Rigbody { get; set; }

    private void Awake()
    {
        Sprite = GetComponentInChildren<SpriteRenderer>();
        Rigbody = GetComponent<Rigidbody2D>();
    }
}