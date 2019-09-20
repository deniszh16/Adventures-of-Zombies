using UnityEngine;

public class Trap : MonoBehaviour
{
    // Ссылки на компоненты
    private Animator animator;
    private SpriteRenderer sprite;
    private AudioSource audioSource;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если персонаж коснулся капкана
        if (collision.gameObject.GetComponent<Character>())
        {
            // Перезапускаем анимацию
            animator.enabled = true;
            animator.Rebind();

            // Если звуки не отключены, проигрываем
            if (Options.sound) audioSource.Play();

            // Повышаем слой объекта
            sprite.sortingOrder += 2;
        }
    }
}