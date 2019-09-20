using UnityEngine;

public class Barrel : MonoBehaviour
{
    // Активность красной бочки
    private bool activeBarrel = true;

    [Header("Эффект уничтожения")]
    [SerializeField] private Animator destruction;

    // Ссылка на звуковой компонент эффекта
    private AudioSource audioSource;

    private Animator animator;
    private CameraShaking cameraShaking;

    private void Awake()
    {
        audioSource = destruction.gameObject.GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        cameraShaking = Camera.main.GetComponent<CameraShaking>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Получаем компонент персонажа у конувшегося объекта
        var character = collision.gameObject.GetComponent<Character>();

        // Если бочка активна
        if (character && activeBarrel)
        {
            // Отключаем бочку от повторных касаний
            activeBarrel = false;
            // Запускаем анимацию уничтожения
            animator.enabled = true;
            animator.Rebind();
        }
    }

    // Уничтожение бочки
    private void DestroyBarrel()
    {
        // Если звуки не отключены, проигрываем
        if (Options.sound) audioSource.Play();

        // Перемещаем эффект уничтожения к бочке
        destruction.transform.position = gameObject.transform.position;
        // Перезапускаем анимацию эффекта
        destruction.Rebind();

        // Отображаем эффект дрожания камеры
        cameraShaking.ShakeCamera();

        // Увеличиваем число уничтоженных бочек
        PlayerPrefs.SetInt("barrel", PlayerPrefs.GetInt("barrel") + 1);

        // Отключаем бочку
        gameObject.SetActive(false);
    }
}