using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("Спрайты кнопок")]
    [SerializeField] protected Sprite[] sprites = new Sprite[2];

    // Ссылка на графический компонент кнопки
    protected Image button;

    protected virtual void Awake()
    {
        button = GetComponent<Image>();
    }

    /// <summary>Настройка параметра игры (номер спрайта кнопки, ключ сохранения, обновленное состояние)</summary>
    protected void ConfigureParameter(int sprite, string saveKey, string state)
    {
        // Меняем спрайт кнопки
        button.sprite = sprites[sprite];

        // Сохраняем обновленное состояние
        PlayerPrefs.SetString(saveKey, state);

        // Обновляем статическую переменную
        Options.ChangeSettingsVariable();
    }
}