using UnityEngine;

public class CameraShaking : MonoBehaviour
{
    // Ссылка на аниматор
    private Animator animator;

    private void Awake() { animator = GetComponent<Animator>(); }

    // Тряска камеры
    public void ShakeCamera()
    {
        // Активируем компонент анимации
        animator.enabled = true;
        // Перезапускаем анимацию
        animator.Rebind();
    }
}