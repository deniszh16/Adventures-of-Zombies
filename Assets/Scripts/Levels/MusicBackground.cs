using UnityEngine;

public class MusicBackground : MonoBehaviour
{
    [Header("Фоновая музыка")]
    [SerializeField] private AudioClip[] backgrounds;

    private AudioSource source;

    private void Awake()
    {
        source = Camera.main.GetComponent<AudioSource>();

        // Устанавливаем случайный музыкальный файл
        source.clip = backgrounds[Random.Range(0, backgrounds.Length)];
    }

    public void SwitchMusic(bool state)
    {
        // Включаем музыку
        if (state) source.Play();
        // Выключаем музыку
        else source.Stop();
    }
}