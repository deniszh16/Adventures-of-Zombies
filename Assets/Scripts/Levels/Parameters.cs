using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Parameters : MonoBehaviour
{
    // Событие по запуску уровня
    public UnityEvent StartLevel { get; } = new UnityEvent();

    // Текущий игровой режим
    public string Mode { get; set; } = "none";

    // Номер текущего уровня
    private int LevelNumber { get; set; }

    [Header("Время на уровень")]
    [SerializeField] private int seconds;

    [Header("Секунды для итоговых звезд")]
    [SerializeField] private int[] finalSeconds;

    // Количество мозгов на уровне
    public int Brains { get; set; } = 3;

    // Количество собранных монет
    public int Coins { get; set; }

    [Header("Список персонажей")]
    [SerializeField] private GameObject[] characters;

    // Ссылка на компонент персонажа
    public Character Character { get; private set; }

    [Header("Стартовая позиция персонажа")]
    [SerializeField] private Vector3 position;

    // Объект для работы со статистикой по персонажам
    private CharactersJson CharactersJson { get; set; } = new CharactersJson();

    [Header("Элементы интерфейса")]
    [SerializeField] private GameObject[] interfaceElements;

    // Индексатор для получения доступа к элементам интерфейса
    public GameObject this[int index] { get { return interfaceElements[index]; } }

    // Перечисление элементов интерфейса
    public enum InterfaceElements
    {
        PauseButton, BrainIcon, StarsIcon, Blackout, HintPanel, Frame,
        PausePanel, LosePanel, VictoryPanel, StartLevel, FinishText, CoinsPanel
    }

    // Варианты победных текстов для количества полученных звезд
    private string[] winningText = { "result-threestars", "result-twostars", "result-onestars" };

    [Header("Звезды за прохождение")]
    [SerializeField] private Sprite[] iconStars;

    // Количество звезд за уровень
    private int Stars { get; set; }

    // Объект для работы со списком полученных звезд за уровни
    private StarsJson StarsJson { get; set; } = new StarsJson();

    [Header("Обучение на уровне")]
    [SerializeField] private bool hint = false;

    // Ссылка на компонент перевода
    public TextTranslation TextHint { get; private set; }

    // Тексты статистики уровня
    private Text textBrains;
    private Text textSeconds;
    // Ссылка на компонент обводки текста
    private Outline textSecondsOutline;

    // Текст о завершении уровня
    public Text TextFinish { get; private set; }

    // Ссылка на компонент фоновой музыки
    private MusicBackground music;

    private void Awake()
    {
        // Активируем выбранного персонажа
        characters[PlayerPrefs.GetInt("character") - 1].SetActive(true);
        // Перемещение персонажа в стартовую точку
        characters[PlayerPrefs.GetInt("character") - 1].transform.position = position;

        // Получаем компонент персонажа
        Character = characters[PlayerPrefs.GetInt("character") - 1].GetComponent<Character>();

        // Преобразуем сохраненную json строку по персонажам в объект
        CharactersJson = JsonUtility.FromJson<CharactersJson>(PlayerPrefs.GetString("character-" + PlayerPrefs.GetInt("character")));
        // Преобразуем сохраненную json строку по звездам в объект
        StarsJson = JsonUtility.FromJson<StarsJson>(PlayerPrefs.GetString("stars-level"));

        // Получение компонентов
        TextHint = interfaceElements[(int)InterfaceElements.HintPanel].transform.GetChild(2).GetComponent<TextTranslation>();
        textBrains = interfaceElements[(int)InterfaceElements.BrainIcon].GetComponentInChildren<Text>();
        textSeconds = interfaceElements[(int)InterfaceElements.StarsIcon].transform.GetChild(3).GetComponent<Text>();
        textSecondsOutline = textSeconds.gameObject.GetComponent<Outline>();
        TextFinish = interfaceElements[(int)InterfaceElements.FinishText].GetComponent<Text>();
        music = Camera.main.GetComponent<MusicBackground>();

        // Получаем номер текущего уровня
        LevelNumber = int.Parse(SceneManager.GetActiveScene().name);
    }

    private void Start()
    {
        // Если активно обучение на уровне
        if (hint && Training.display)
        {
            // Активируем отображение обучающей панели
            Invoke("PlayerTraining", 1.5f);
        }
        else
        {
            // Запускаем уровень
            RunLevel();
        } 
    }

    /// <summary>Отображение обучающей панели на уровне</summary>
    private void PlayerTraining()
    {
        // Переключаем режим
        Mode = "training";

        // Активируем компонент обучения
        Camera.main.GetComponent<Training>().enabled = true;
    }

    /// <summary>Старт игрового уровня</summary>
    public void RunLevel()
    {
        // Скрываем панель затемнения и панель подсказок
        interfaceElements[(int)InterfaceElements.Blackout].SetActive(false);
        interfaceElements[(int)InterfaceElements.HintPanel].SetActive(false);

        // Отображаем кнопку паузы
        interfaceElements[(int)InterfaceElements.PauseButton].SetActive(true);

        // Активируем игровой режим
        Mode = "play";

        // Запускаем подсчет времени
        StartCoroutine(CountTime());

        // Вызываем методы, запускаемые после старта
        StartLevel.Invoke();

        // Если звуки не отключены, запускаем музыку
        if (Options.sound) music.SwitchMusic(true);
    }

    /// <summary>Подсчет времени прохождения</summary>
    private IEnumerator CountTime()
    {
        while (Mode == "play")
        {
            if (seconds > 0)
            {
                yield return new WaitForSeconds(1);
                // Уменьшаем секунды
                seconds--;
                // Обновляем текстовый счетчик
                textSeconds.text = seconds.ToString();

                // Если секунд меньше, чем нужно для получения указанного количества звезд
                if (seconds < finalSeconds[Stars])
                {
                    // Убираем звезду с экрана
                    interfaceElements[(int)InterfaceElements.StarsIcon].transform.GetChild(Stars).gameObject.SetActive(false);
                    // Увеличиваем количество потерянных звезд
                    Stars++;

                    // Если недоступно две звезды
                    if (Stars == 2)
                        // Перекрашиваем таймер в красный цвет
                        textSecondsOutline.effectColor = new Color32(255, 0, 0, 128);
                }
            }
            else
            {
                // Если секунды закончились, уничтожаем персонажа
                Character.RecieveDamageCharacter(false, true, 1.5f);
                // Отключаем кнопку воскрешения на уровне
                interfaceElements[(int)InterfaceElements.LosePanel].transform.GetChild(0).GetComponent<Button>().interactable = false;
            }
        }
    }

    /// <summary>Обновление количества мозгов</summary>
    public void RefreshQuantityBrains()
    {
        // Если есть не собранные мозги
        if (Brains > 0)
        {
            // Выводим их количество
            textBrains.text = "x" + Brains.ToString();
        }
        else
        {
            // Выводим сообщение о полном сборе
            textBrains.text = "ok";
            // Перекрашиваем текст в зеленый цвет
            textBrains.color = new Color32(127, 255, 0, 255);
        }
    }

    /// <summary>Вывод результатов уровня</summary>
    public void ShowResults()
    {
        // Останавливаем подсчет времени
        StopCoroutine("CountTime");

        // Отображаем панель затемнения, панель подсказок и рамку для кнопок
        interfaceElements[(int)InterfaceElements.Blackout].SetActive(true);
        interfaceElements[(int)InterfaceElements.HintPanel].SetActive(true);
        interfaceElements[(int)InterfaceElements.Frame].SetActive(true);

        // Увеличиваем количество игр для персонажа
        CharactersJson.played++;

        // Обновляем текущее количество монет
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + Coins);
        // Обновляем собранное количество монет за всё время
        PlayerPrefs.SetInt("piggybank", PlayerPrefs.GetInt("piggybank") + Coins);

        // Если уровень пройден
        if (Mode == "victory")
        {
            // Если сохраненный прогресс меньше номера текущего уровня, увеличиваем прогресс
            if (PlayerPrefs.GetInt("progress") <= LevelNumber) PlayerPrefs.SetInt("progress", ++LevelNumber);

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

            // Выводим победный текст
            TextHint.ChangeKey(winningText[Stars]);
            // Отображаем полученные звезды за уровень
            interfaceElements[(int)InterfaceElements.VictoryPanel].transform.GetChild(0).GetComponent<Image>().sprite = iconStars[Stars];
            // Пересчитываем потерянные звезды в полученные
            Stars = 3 - Stars;

            // Если доступен интернет
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                // Если любой (кроме первого) уровень пройден на три звезды
                if (Stars == 3 && LevelNumber > 1)
                    // Разблокируем достижение (как много звёзд)
                    PlayServices.UnlockingAchievement(GPGSIds.achievement_2);
            }

            // Записываем звездный результат
            GettingStars(Stars);

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
                    // Отключаем кнопку воскрешения персонажа
                    interfaceElements[(int)InterfaceElements.LosePanel].transform.GetChild(0).GetComponent<Button>().interactable = false;
            }

            // Отображаем общее количество монет
            interfaceElements[(int)InterfaceElements.CoinsPanel].SetActive(true);
            // Обновляем общее количество монет
            interfaceElements[(int)InterfaceElements.CoinsPanel].transform.GetChild(1).GetComponent<Text>().text = PlayerPrefs.GetInt("coins").ToString();

            // Выводим проигрышный текст
            TextHint.ChangeKey("result-lose");

            // Увеличиваем количество проигрышей для персонажа
            CharactersJson.loss++;
        }

        // Сохраняем статистику по активному персонажу
        PlayerPrefs.SetString("character-" + PlayerPrefs.GetInt("character"), JsonUtility.ToJson(CharactersJson));
    }

    /// <summary>Запись результата по звездам (количество звезд)</summary>
    private void GettingStars(int quantity)
    {
        // Если в списке меньше значений, чем номер уровня
        if (StarsJson.stars.Count < LevelNumber)
        {
            // Создаем новый элемент с количеством полученных звезд
            StarsJson.stars.Add(quantity);
            // Сохраняем значение
            PlayerPrefs.SetString("stars-level", JsonUtility.ToJson(StarsJson));
        }
        // Иначе проверяем, меньше ли старое значение нового
        else if (StarsJson.stars[LevelNumber - 1] < quantity)
        {
            // Записываем новое значение и сохраняем
            StarsJson.stars[LevelNumber - 1] = quantity;
            PlayerPrefs.SetString("stars-level", JsonUtility.ToJson(StarsJson));
        }
    }
}