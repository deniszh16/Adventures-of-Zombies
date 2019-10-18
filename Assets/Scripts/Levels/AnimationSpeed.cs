using UnityEngine;

public class AnimationSpeed : MonoBehaviour
{
    [Header("Скорость анимации")]
    [SerializeField] private float speed;

    [Header("Рандомная скорость")]
    [SerializeField] private bool random = false;

    [Header("Ограничения скорости")]
    [SerializeField] private float[] intervalSpeed;

    // Ссылка на аниматор
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Устанавливаем рандомную или указанную скорость проигрывания анимации
        animator.speed = random ? Random.Range(intervalSpeed[0], intervalSpeed[1]) : speed;
    }
}