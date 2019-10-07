using System.Collections;
using UnityEngine;

public class SwitchMusic : MonoBehaviour
{
    // Ссылки на компоненты
    private BackgroundMusic backgroundMusic;
    private AudioSource audioSource;

    private void Start()
    {
        backgroundMusic = GameObject.FindGameObjectWithTag("Music").GetComponent<BackgroundMusic>();
        audioSource = backgroundMusic.gameObject.GetComponent<AudioSource>();

        // Если звуки не отключены, но музыка не проигрывается
        if (Options.sound && !audioSource.isPlaying)
            // Запускаем восстановление музыки
            StartCoroutine(RestoreMusic());
    }

    // Отключение музыки при запуске уровня
    public void StopMusic()
    {
        // Запускаем затухание музыки
        StartCoroutine(MuteMusic());
    }

    public IEnumerator MuteMusic()
    {
        // Пока громкость больше нуля
        while (audioSource.volume > 0)
        {
            yield return new WaitForSeconds(0.03f);
            // Уменьшаем громкость
            audioSource.volume -= 0.01f;
        }

        // Отключаем проигрывание музыки
        backgroundMusic.StopMenuMusic(); 
    }

    // Восстановление музыки
    public IEnumerator RestoreMusic()
    {
        while (audioSource.volume < 0.65f)
        {
            yield return new WaitForSeconds(0.03f);
            // Увеличиваем громкость
            audioSource.volume += 0.01f;
        }

        // Воспроизводим музыку
        backgroundMusic.PlayMenuMusic();
    }
}