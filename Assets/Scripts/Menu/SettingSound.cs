public class SettingSound : Settings
{
    private void Start()
    {
        // Если звук отключен, заменяем спрайт кнопки
        if (!Options.sound) button.sprite = sprites[1];
    }

    // Переключение звука
    public void SwitchSounds()
    {
        // Если звуки активны, настраиваем отключение
        if (Options.sound) ConfigureParameter(1, "sounds", "false");
        // Иначе настраиваем включение звуков
        else ConfigureParameter(0, "sounds", "true");
    }
}