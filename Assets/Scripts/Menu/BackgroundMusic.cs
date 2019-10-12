using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    // Ссылка на музыкальный компонент
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Запускаем фоновую музыку
        PlayMenuMusic();
    }

    // Запуск фоновой музыки
    public void PlayMenuMusic()
    {
        if (Options.sound)
            audioSource.Play();
    }

    // Остановка фоновой музыки
    public void StopMenuMusic()
    {
        audioSource.Stop();
    }
}