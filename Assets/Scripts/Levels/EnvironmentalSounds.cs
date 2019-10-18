using UnityEngine;

public class EnvironmentalSounds : MonoBehaviour
{
    // Ссылка на музыкальный компонент
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // Если звуки не отключены
        if (Options.sound)
            // Проигрываем звук
            audioSource.Play();
    }
}