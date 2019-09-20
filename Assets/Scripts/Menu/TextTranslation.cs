using UnityEngine;
using UnityEngine.UI;

public class TextTranslation : MonoBehaviour
{
    [Header("Автоматический перевод")]
    [SerializeField] private bool auto = true;

    [Header("Ключ перевода")]
    [SerializeField] private string key;

    private Text textComponent;

    private void Awake() { textComponent = GetComponent<Text>(); }

    private void Start()
    {
        // Автоматический перевод
        if (auto) TranslateText();
    }

    // Обновление перевода
    public void TranslateText()
    {
        // Установка текста с указанным ключом из xml файла
        textComponent.text = ParseTranslation.languages.Element("languages").Element(Options.language).Element(key).Value;
    }

    // Изменение ключа и обновление перевода
    public void ChangeKey(string value) { key = value; TranslateText(); }
}