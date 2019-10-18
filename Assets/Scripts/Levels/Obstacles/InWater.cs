using UnityEngine;

public class InWater : MonoBehaviour
{
    [Header("Масса в воде")]
    [SerializeField] private float mass;

    // Свойство для получения массы объекта в воде
    public float Mass { get { return mass; } }

    [Header("Отображение эффекта")]
    [SerializeField] private bool spray = false;

    // Свойство для получения информации по эффекту брызг
    public bool Spray { get { return spray; } }

    [Header("Проигрывание звука")]
    [SerializeField] private bool playingSound = true;

    // Свойство для получения информации по звуковому эффекту
    public bool PlayingSound { get { return playingSound; } }

    // Ссылки на используемые компоненты
    public SpriteRenderer Sprite { get; private set; }
    public Rigidbody2D Rigbody { get; private set; }

    private void Awake()
    {
        Sprite = GetComponentInChildren<SpriteRenderer>();
        Rigbody = GetComponent<Rigidbody2D>();
    }
}