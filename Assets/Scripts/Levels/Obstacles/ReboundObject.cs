using UnityEngine;

public class ReboundObject : MonoBehaviour
{
    [Header("Вектор отскока")]
    [SerializeField] protected Vector2 rebound;

    [Header("Сила отскока")]
    [SerializeField] protected float force;

    protected Animator animator;
    protected AudioSource audioSource;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // Пытаемся получить компонент физики у коснувшегося объекта
        Rigidbody2D rigbody = collision.gameObject.GetComponent<Rigidbody2D>();

        if (rigbody)
        {
            // Создаем импульсный отскок объекта по вектору
            rigbody.AddForce(rebound * force, ForceMode2D.Impulse);

            // Если звуки не отключены, проигрываем звук
            if (Options.sound) audioSource.Play();
            
            // Активируем и перезапускаем анимацию
            animator.enabled = true;
            animator.Rebind();
        }
    }
}