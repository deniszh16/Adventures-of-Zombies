using System.Collections;
using UnityEngine;

public class MusicBackground : MonoBehaviour
{
    [Header("Фоновая музыка")]
    [SerializeField] private AudioClip[] backgrounds;

    // Ссылка на музыкальный компонент
    private AudioSource source;

    private void Awake()
    {
        source = Camera.main.GetComponent<AudioSource>();

        // Устанавливаем случайный музыкальный файл
        source.clip = backgrounds[Random.Range(0, backgrounds.Length)];
    }

    /// <summary>Переключение фоновой музыки (состояние музыки)</summary>
    public void SwitchMusic(bool state)
    {
        if (state)
        {
            // Включаем музыку
            source.Play();
            // Запускаем плавное увеличение громкости
            StartCoroutine(IncreaseVolume());
        }
        else
        {
            // Останавливаем музыку
            source.Stop();
        }
    }

    /// <summary>Плавное увеличение громкости</summary>
    private IEnumerator IncreaseVolume()
    {
        // Пока громкость ниже указанного значения
        while (source.volume < 0.3f)
        {
            yield return new WaitForSeconds(0.05f);
            // Увеличиваем громкость
            source.volume += 0.01f;
        }
    }
}