using UnityEngine;

public class Options : MonoBehaviour
{
    // Активность музыки
    public static bool sound;

    // Язык интерфейса игры
    public static string language;

    private void Start()
    {
        ChangeSettingsVariable();
    }

    /// <summary>Установка игровых параметров</summary>
    public static void ChangeSettingsVariable()
    {
        // Устанавливаем значения в зависимости от сохраненных параметров
        sound = (PlayerPrefs.GetString("sounds") == "true") ? true : false;
        language = (PlayerPrefs.GetString("language") == "ru") ? "ru" : "en";
    }
}