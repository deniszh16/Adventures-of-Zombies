using UnityEngine;

public class CameraShaking : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>Тряска камеры</summary>
    public void ShakeCamera()
    {
        // Активируем анимацию
        animator.enabled = true;
        // Перезапускаем анимацию
        animator.Rebind();
    }
}