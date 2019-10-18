using System.Collections;
using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    // Ссылка на музыкальный компонент
    private AudioSource audioSource;

    // Перечисление состояний музыки
    private enum State { Stopped, Playing }

    private void Start()
    {
        // Находим и получаем компонент фоновой музыки в меню
        audioSource = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();

        // Если музыка не отключена в настройках, но не проигрывается
        if (Options.sound && !audioSource.isPlaying)
            // Вызываем запуск музыки
            StartBackgroundMusic();
    }

    /// <summary>Запуск фоновой музыки в меню</summary>
    public void StartBackgroundMusic()
    {
        StartCoroutine(ChangeVolume(State.Playing));
    }

    /// <summary>Остановка фоновой музыки</summary>
    public void StopBackgroundMusic()
    {
        StartCoroutine(ChangeVolume(State.Stopped));
    }

    /// <summary>Изменение громкости музыки (состояние музыки)</summary>
    private IEnumerator ChangeVolume(State state)
    {
        // Если нужно включить музыку
        if (state == State.Playing)
        {
            // Запускаем проигрывание
            audioSource.Play();

            // Пока громкость ниже указанного значения
            while (audioSource.volume < 1f)
            {
                yield return new WaitForSeconds(0.02f);
                // Постепенно увеличиваем громкость
                audioSource.volume += 0.01f;
            }
        }
        else
        {
            // Пока громкость выше указанного значения
            while (audioSource.volume > 0)
            {
                yield return new WaitForSeconds(0.02f);
                // Постепенно уменьшаем громкость
                audioSource.volume -= 0.03f;
            }

            // Останавливаем проигрывание
            audioSource.Stop();
        }  
    }
}