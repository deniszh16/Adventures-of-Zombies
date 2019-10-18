using UnityEngine;
using UnityEngine.UI;

public class TextTranslation : MonoBehaviour
{
    [Header("Автоматический перевод")]
    [SerializeField] private bool auto = true;

    [Header("Ключ перевода")]
    [SerializeField] private string key;

    // Ссылка на текстовый компонент
    private Text textComponent;

    private void Awake()
    {
        textComponent = GetComponent<Text>();
    }

    private void Start()
    {
        // Если установлен автоперевод
        if (auto)
            // Переводим текст
            TranslateText();
    }

    /// <summary>Обновление перевода</summary>
    public void TranslateText()
    {
        // Устанавливаем текст с указанным ключом из xml файла
        textComponent.text = ParseTranslation.languages.Element("languages").Element(Options.language).Element(key).Value;
    }

    /// <summary>Изменение ключа и обновление перевода (новый ключ перевода)</summary>
    public void ChangeKey(string value)
    {
        key = value;
        TranslateText();
    }
}