using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Parameters : MonoBehaviour
{
    // Событие по запуску уровня
    public UnityEvent StartLevel { get; set; } = new UnityEvent();

    // Текущий игровой режим
    public string Mode { get; set; } = "none";

    // Номер текущего уровня
    private int levelNumber;

    [Header("Время на уровень")]
    public int seconds;

    [Header("Секунды для звезд")]
    [SerializeField] private int[] finalSeconds;

    // Количество собранных мозгов
    public int Brains { get; set; } = 3;

    // Количество собранных монет
    public int Coins { get; set; }

    [Header("Список персонажей")]
    [SerializeField] private GameObject[] characters;

    // Ссылка на компонент персонажа
    public Character Character { get; set; }

    [Header("Стартовая позиция персонажа")]
    [SerializeField] private Vector3 position;

    // Объект для работы с json по персонажам
    private CharactersJson charactersJson = new CharactersJson();

    [Header("Элементы интерфейса")]
    public GameObject[] interfaceElements;

    // Перечисление элементов интерфейса
    public enum InterfaceElements
    {
        PauseButton,
        BrainIcon,
        StarsIcon,
        Blackout,
        HintPanel,
        Frame,
        PausePanel,
        LosePanel,
        VictoryPanel,
        StartLevel,
        FinishText,
        CoinsPanel
    }

    // Варианты победных текстов
    private string[] winningText = { "result-threestars", "result-twostars", "result-onestars" };

    [Header("Звезды за прохождение")]
    [SerializeField] private Sprite[] iconStars;

    // Объект для работы с json по звездам
    private StarsJson starsJson = new StarsJson();

    [Header("Обучение на уровне")]
    [SerializeField] private bool hint = false;

    public TextTranslation TextHint { get; set; }
    private Text textBrains;
    private Text textSeconds;
    private Outline textSecondsOutline;
    public Text TextFinish { get; set; }
    private MusicBackground music;
    private AudioSource audioSource;

    private void Awake()
    {
        // Активируем выбранного персонажа
        characters[PlayerPrefs.GetInt("character") - 1].SetActive(true);
        // Перемещение персонажа в стартовую точку
        characters[PlayerPrefs.GetInt("character") - 1].transform.position = position;

        // Получаем компонент персонажа
        Character = characters[PlayerPrefs.GetInt("character") - 1].GetComponent<Character>();

        // Преобразуем сохраненную json строку по персонажам в объект
        charactersJson = JsonUtility.FromJson<CharactersJson>(PlayerPrefs.GetString("character-" + PlayerPrefs.GetInt("character")));
        // Преобразуем сохраненную json строку по звездам в объект
        starsJson = JsonUtility.FromJson<StarsJson>(PlayerPrefs.GetString("stars-level"));

        // Получение компонентов
        textBrains = interfaceElements[(int)InterfaceElements.BrainIcon].GetComponentInChildren<Text>();
        textSeconds = interfaceElements[(int)InterfaceElements.StarsIcon].transform.GetChild(3).GetComponent<Text>();
        textSecondsOutline = textSeconds.gameObject.GetComponent<Outline>();
        TextFinish = interfaceElements[(int)InterfaceElements.FinishText].GetComponent<Text>();
        music = Camera.main.GetComponent<MusicBackground>();
        audioSource = Camera.main.GetComponent<AudioSource>();
        TextHint = interfaceElements[(int)InterfaceElements.HintPanel].transform.GetChild(2).GetComponent<TextTranslation>();

        // Получаем номер текущего уровня
        levelNumber = int.Parse(SceneManager.GetActiveScene().name);
    }

    private void Start()
    {
        // Если обучение доступно, активируем компонент обучения
        if (hint && Training.display) Invoke("PlayerTraining", 1.5f);
        // Иначе запускаем уровень
        else RunLevel();
    }

    // Запуск обучения игрока
    private void PlayerTraining()
    {
        // Переключаем режим на обучение
        Mode = "training";
        // Активируем компонент обучения
        Camera.main.GetComponent<Training>().enabled = true;
    }

    // Запуск уровня
    public void RunLevel()
    {
        // Скрываем панель затемнения и панель подсказок
        interfaceElements[(int)InterfaceElements.Blackout].SetActive(false);
        interfaceElements[(int)InterfaceElements.HintPanel].SetActive(false);

        // Показываем кнопку паузы
        interfaceElements[(int)InterfaceElements.PauseButton].SetActive(true);

        // Активируем игровой режим
        Mode = "play";

        // Запускаем подсчет времени
        StartCoroutine(CountTime());

        // Вызываем зарегистрированные методы
        StartLevel.Invoke();

        // Если звуки не отключены, запускаем фоновую музыку
        if (Options.sound) music.SwitchMusic(true);
        // Постепенно увеличиваем громкость
        StartCoroutine(IncreaseVolume());
    }

    private IEnumerator IncreaseVolume()
    {
        // Пока громкость ниже указанного значения
        while (audioSource.volume < 0.35f)
        {
            yield return new WaitForSeconds(0.05f);
            // Увеличиваем громкость
            audioSource.volume += 0.01f;
        }
    }

    // Таймер на уровне
    public IEnumerator CountTime()
    {
        // Проверка секунд
        var secondsCheck = 0;

        // Пока активен игровой режим
        while (Mode == "play")
        {
            // Если есть секунды
            if (seconds > 0)
            {
                // Отсчитываем секунды прохождения
                yield return new WaitForSeconds(1);
                seconds--;
                // Обновляем счетчик на экране
                textSeconds.text = seconds.ToString();

                // Если секунд меньше указанного количества
                if (seconds < finalSeconds[secondsCheck])
                {
                    // Убираем звезду с экрана
                    interfaceElements[(int)InterfaceElements.StarsIcon].transform.GetChild(secondsCheck).gameObject.SetActive(false);
                    // Увеличиваем номер проверки
                    secondsCheck++;

                    // Проверяем цвета таймера
                    ChangeTextColor();
                }
            }
            else
            {
                // Если секунды закончились, уничтожаем персонажа
                Character.RecieveDamageCharacter(false, true, 1.5f);
                // Отключаем кнопку воскрешения
                interfaceElements[(int)InterfaceElements.LosePanel].transform.GetChild(0).GetComponent<Button>().interactable = false;
            }
        }
    }

    // Изменение цвета таймера
    public void ChangeTextColor()
    {
        // Если секунды кончаются, перекрашиваем таймер в красный цвет, иначе оставляем стандартный черный
        textSecondsOutline.effectColor = (seconds <= finalSeconds[1]) ? new Color32(255, 0, 0, 128) : new Color32(0, 0, 0, 128);
    }

    // Обновление количества мозгов
    public void RefreshQuantityBrains()
    {
        // Если мозгов больше нуля, выводим количество
        if (Brains > 0) textBrains.text = "x" + Brains.ToString();
        else
        {
            // Выводим сообщение о полном сборе мозгов
            textBrains.text = "ok";
            // Перекрашиваем текст в зеленый цвет
            textBrains.color = new Color32(127, 255, 0, 255);
        }
    }

    // Вывод результатов
    public void ShowResults()
    {
        // Останавливаем подсчет времени
        StopCoroutine("CountTime");

        // Отображаем панель затемнения, панель подсказок и рамку для кнопок
        interfaceElements[(int)InterfaceElements.Blackout].SetActive(true);
        interfaceElements[(int)InterfaceElements.HintPanel].SetActive(true);
        interfaceElements[(int)InterfaceElements.Frame].SetActive(true);

        // Увеличиваем количество игр для персонажа
        charactersJson.played++;

        // Обновляем количество собранных монет
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + Coins);
        // Обновляем собранное количество монет за всё время
        PlayerPrefs.SetInt("piggybank", PlayerPrefs.GetInt("piggybank") + Coins);

        // Если уровень пройден
        if (Mode == "victory")
        {
            // Если сохраненный прогресс меньше номера текущего уровня, увеличиваем прогресс
            if (PlayerPrefs.GetInt("progress") <= levelNumber) PlayerPrefs.SetInt("progress", ++levelNumber);

            // Отображаем панель победы
            interfaceElements[(int)InterfaceElements.VictoryPanel].SetActive(true);

            // Подсчитываем заработанные очки на уровне
            var points = seconds * 50 + Random.Range(5, 50);
            // Выводим набранное количество очков
            interfaceElements[(int)InterfaceElements.VictoryPanel].transform.GetChild(4).GetComponent<Text>().text = points.ToString();
            // Обновляем общее количество очков
            PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + points);

            // Обновляем общее количество собранных мозгов
            PlayerPrefs.SetInt("brains", PlayerPrefs.GetInt("brains") + Brains);

            // Количество звезд
            var stars = 0;

            for (int i = 0; i < finalSeconds.Length; i++)
            {
                // Если оставшиеся секунды больше указанного значения
                if (seconds >= finalSeconds[i])
                {
                    // Записываем количество звезд за уровень
                    stars = 4 - (i + 1);

                    // Выводим соответствующий текст
                    TextHint.ChangeKey(winningText[i]);
                    // Отображаем полученные звезды за уровень
                    interfaceElements[(int)InterfaceElements.VictoryPanel].transform.GetChild(0).GetComponent<Image>().sprite = iconStars[i];
                    break;
                }
            }

            // Если доступен интернет
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                // Если уровень пройден на три звезды
                if (stars == 3 && levelNumber > 1)
                    // Разблокируем достижение (как много звёзд)
                    PlayServices.UnlockingAchievement(GPGSIds.achievement_2);
            }

            // Записываем результат по звездам
            GettingStars(stars);
        }
        else if (Mode == "lose")
        {
            // Отображаем панель поражения
            interfaceElements[(int)InterfaceElements.LosePanel].SetActive(true);

            // Если недостаточно монет
            if (PlayerPrefs.GetInt("coins") < 50)
            {
                // Если недоступен интернет
                if (Application.internetReachability == NetworkReachability.NotReachable)
                    // Находим кнопку воскрешения и отключаем ее
                    interfaceElements[(int)InterfaceElements.LosePanel].transform.GetChild(0).GetComponent<Button>().interactable = false;
            }

            // Отображаем общее количество монет
            interfaceElements[(int)InterfaceElements.CoinsPanel].SetActive(true);
            // Обновляем общее количество монет
            interfaceElements[(int)InterfaceElements.CoinsPanel].transform.GetChild(1).GetComponent<Text>().text = PlayerPrefs.GetInt("coins").ToString();

            // Выводим текст проигрыша
            TextHint.ChangeKey("result-lose");

            // Увеличиваем количество проигрышей для персонажа
            charactersJson.loss++;
        }

        // Сохраняем статистику по активному персонажу
        PlayerPrefs.SetString("character-" + PlayerPrefs.GetInt("character"), JsonUtility.ToJson(charactersJson));
    }

    // Запись результата по звездам
    private void GettingStars(int quantity)
    {
        // Если в списке меньше значений, чем номер уровня
        if (starsJson.stars.Count < levelNumber)
        {
            // Создаем новый элемент с количеством полученных звезд
            starsJson.stars.Add(quantity);
            // Сохраняем значение
            PlayerPrefs.SetString("stars-level", JsonUtility.ToJson(starsJson));
        }
        // Иначе проверяем, меньше ли старое значение нового
        else if (starsJson.stars[levelNumber - 1] < quantity)
        {
            // Записываем новое значение и сохраняем
            starsJson.stars[levelNumber - 1] = quantity;
            PlayerPrefs.SetString("stars-level", JsonUtility.ToJson(starsJson));
        }
    }
}