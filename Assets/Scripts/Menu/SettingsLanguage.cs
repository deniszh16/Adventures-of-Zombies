using UnityEngine;

public class SettingsLanguage : Settings
{
    // Компонент перевода у заголовка
    private TextTranslation title;

    protected override void Awake()
    {
        base.Awake();

        // Находим заголовок и получаем компонент перевода
        title = GameObject.FindWithTag("Title").GetComponent<TextTranslation>();
    }

    private void Start()
    {
        // Если установлен английский язык, заменяем спрайт кнопки
        if (Options.language == "en") button.sprite = sprites[1];
    }

    /// <summary>Переключение языка интерфейса</summary>
    public void SwitchLanguage()
    {
        // Если активен русский язык, выполняем переключение на английский
        if (Options.language == "ru") ConfigureParameter(1, "language", "en");
        // Иначе переключаемся на русский
        else ConfigureParameter(0, "language", "ru");

        // Обновляем перевод заголовка
        title.TranslateText();
    }
}