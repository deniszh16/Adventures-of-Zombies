using UnityEngine;

public class SettingSound : Settings
{
    // Сссылка на фоновую музыку в меню
    private MenuMusic menuMusic;

    private void Start()
    {
        menuMusic = Camera.main.GetComponent<MenuMusic>();

        // Если звук отключен, заменяем спрайт кнопки
        if (!Options.sound) button.sprite = sprites[1];
    }

    /// <summary>Переключение звука</summary>
    public void SwitchSounds()
    {
        // Если звуки активны
        if (Options.sound)
        {
            // Настраиваем отключение
            ConfigureParameter(1, "sounds", "false");
            // Отключаем фоновую музыку
            menuMusic.StopBackgroundMusic();
        }
        else
        {
            // Иначе настраиваем включение звуков
            ConfigureParameter(0, "sounds", "true");
            // Запускаем фоновую музыку
            menuMusic.StartBackgroundMusic();
        } 
    }
}