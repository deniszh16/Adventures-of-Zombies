using UnityEngine;
using UnityEngine.UI;

public class OpeningCharacter : MonoBehaviour
{
    [Header("Номер персонажа")]
    [SerializeField] private int number;

    [Header("Прогресс для открытия")]
    [SerializeField] private int progress;

    [Header("Части персонажа")]
    [SerializeField] private GameObject parts;

    [Header("Изображение персонажа")]
    [SerializeField] private GameObject zombie;

    [Header("Количество игр")]
    [SerializeField] private Text played;

    [Header("Количество поражений")]
    [SerializeField] private Text loss;

    [Header("Кнопка выбора")]
    [SerializeField] private Button select;

    [Header("Идентификатор достижения")]
    [SerializeField] private string identifier;

    // Текст кнопки выбора персонажа
    private TextTranslation textSelect;

    // Объект для работы с Json по персонажам
    private CharactersJson characters { get; set; } = new CharactersJson();

    private void Awake()
    {
        // Преобразовываем json строку в объект
        characters = JsonUtility.FromJson<CharactersJson>(PlayerPrefs.GetString("character-" + number));

        textSelect = select.GetComponentInChildren<TextTranslation>();
    }

    private void Start()
    {
        // Если персонажа нужно открывать, выполняем его проверку
        if (progress > 0) CheckCharacter();
        else
        {
            // Иначе выводим его статистику
            ShowStatisticsCharacter();
            // Проверяем кнопку выбора
            CheckSelectButton();
        }
    }

    // Проверка персонажа
    private void CheckCharacter()
    {
        // Если прогресс достаточный для открытия
        if (progress <= PlayerPrefs.GetInt("progress"))
        {
            // Открываем персонажа
            ChoiceCharacter.characters[number - 1] = true;

            // Скрываем фрагменты
            parts.SetActive(false);
            // Отображаем персонажа
            zombie.SetActive(true);

            // Отображаем статистику
            ShowStatisticsCharacter();

            // Проверяем кнопку выбора
            CheckSelectButton();
        }
        // Обновляем перевод на закрытой кнопке
        else textSelect.TranslateText();
    }

    // Отображение статистики
    private void ShowStatisticsCharacter()
    {
        // Отображаем и выводим игры
        played.color = Color.white;
        played.text = ParseTranslation.languages.Element("languages").Element(Options.language).Element("statistics-played").Value + " " + characters.played;

        // Если доступен интернет
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            // Если за персонажа сыграно более 15 игр, разблокируем достижение
            if (characters.played >= 15) PlayServices.UnlockingAchievement(identifier);
        }

        // Отображаем и выводим поражения
        loss.color = Color.white;
        loss.text = ParseTranslation.languages.Element("languages").Element(Options.language).Element("statistics-losses").Value + " " + characters.loss;
    }

    // Проверка кнопки выбора
    public void CheckSelectButton()
    {
        // Если персонаж не выбран
        if (number != PlayerPrefs.GetInt("character"))
        {
            // Активируем кнопку выбора
            select.interactable = true;
            // Обновляем текст на кнопке
            textSelect.ChangeKey("button-select");
        }
        else
        {
            // Отключаем кнопку выбора
            select.interactable = false;
            // Иначе меняем текст (персонаж выбран)
            textSelect.ChangeKey("button-selected");
        }
    }
}