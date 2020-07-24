using UnityEngine;
using UnityEngine.UI;
using Cubra.Helpers;

namespace Cubra
{
    public class LevelResults : MonoBehaviour
    {
        // Объект для работы со списком полученных звезд
        private StarsHelper _starsHelper;

        [Header("Панель проигрыша")]
        [SerializeField] private GameObject _losePanel;

        [Header("Кнопка воскрешения")]
        [SerializeField] private Button _resurrect;

        [Header("Количество монет")]
        [SerializeField] private Text _quantityCoins;

        [Header("Панель победы")]
        [SerializeField] private GameObject _victoryPanel;

        [Header("Счет уровня")]
        [SerializeField] private Text _levelScore;

        [Header("Звезды за уровень")]
        [SerializeField] private Image _levelStars;

        [Header("Спрайты звезд")]
        [SerializeField] private Sprite[] _spritesStars;

        [Header("Панель подсказок")]
        [SerializeField] private GameObject _hintPanel;

        public GameObject HintPanel => _hintPanel;

        [Header("Текст подсказки")]
        [SerializeField] private TextTranslation _textHint;

        public TextTranslation TextHint => _textHint;

        // Список победных текстов
        private readonly string[] _winningTexts = { "result-onestars", "result-twostars", "result-threestars" };

        private void Start()
        {
            // Преобразуем сохраненную json строку по звездам в объект
            _starsHelper = JsonUtility.FromJson<StarsHelper>(PlayerPrefs.GetString("stars-level"));

            // Подписываем отображение проигрыша в событие смерти персонажа
            Main.Instance.CharacterController.IsDead += FailedLevel;
        }

        /// <summary>
        /// Завершение уровня после сбора всех мозгов
        /// </summary>
        public void CompleteLevel()
        {
            // Переключаем режим игры на успешное завершение
            Main.Instance.CurrentMode = Main.GameModes.Completed;
            Invoke("WinningAtLevel", 0.7f);
        }

        /// <summary>
        /// Отображение результата после проигрыша
        /// </summary>
        public void FailedLevel()
        {
            // Переключаем режим игры на проигрыш
            Main.Instance.CurrentMode = Main.GameModes.Losing;
            Invoke("LosingAtLevel", 1.5f);
        }

        /// <summary>
        /// Победа на уровне
        /// </summary>
        public void WinningAtLevel()
        {
            UpdateSavedData();

            _victoryPanel.SetActive(true);
            _hintPanel.SetActive(true);

            // Если сохраненный прогресс меньше номера текущего уровня
            if (PlayerPrefs.GetInt("progress") <= Main.Instance.LevelNumber)
                // Увеличиваем прогресс
                PlayerPrefs.SetInt("progress", Main.Instance.LevelNumber + 1);

            // Набранные на уровне очки
            var points = Main.Instance.Timer.Seconds * 55;
            _levelScore.text = points.ToString();
            PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + points);

            // Выводим победный текст
            _textHint.ChangeKey(_winningTexts[Main.Instance.Stars - 1]);

            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                if (Main.Instance.LevelNumber > 1 && Main.Instance.Stars == 3)
                    GooglePlayServices.UnlockingAchievement(GPGSIds.achievement_2);
            }

            // Сохраняем обновленную статистику по персонажу
            PlayerPrefs.SetString("character-" + PlayerPrefs.GetInt("character"), JsonUtility.ToJson(Main.Instance.ZombieHelper));

            // Выводим полученные звезды за уровень
            _levelStars.sprite = _spritesStars[Main.Instance.Stars - 1];
            SaveStars();
        }

        /// <summary>
        /// Запись результата по звездам
        /// </summary>
        private void SaveStars()
        {
            // Если в списке меньше значений, чем номер уровня
            if (_starsHelper.Stars.Count < Main.Instance.LevelNumber)
            {
                // Создаем новый элемент с количеством полученных звезд
                _starsHelper.Stars.Add(Main.Instance.Stars);
                PlayerPrefs.SetString("stars-level", JsonUtility.ToJson(_starsHelper));
            }
            else if (_starsHelper.Stars[Main.Instance.LevelNumber - 1] < Main.Instance.Stars)
            {
                // Записываем новое значение и сохраняем
                _starsHelper.Stars[Main.Instance.LevelNumber - 1] = Main.Instance.Stars;
                PlayerPrefs.SetString("stars-level", JsonUtility.ToJson(_starsHelper));
            }
        }

        /// <summary>
        /// Проигрыш на уровне
        /// </summary>
        private void LosingAtLevel()
        {
            UpdateSavedData();

            _losePanel.SetActive(true);
            _hintPanel.SetActive(true);

            _quantityCoins.text = PlayerPrefs.GetInt("coins").ToString();

            // Если недостаточно монет
            if (PlayerPrefs.GetInt("coins") < 50)
            {
                // И недоступен интернет
                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    // Отключаем кнопку воскрешения
                    _resurrect.interactable = false;
                }    
            }

            // Выводим проигрышный текст
            _textHint.ChangeKey("result-lose");

            // Увеличиваем количество смертей персонажа
            Main.Instance.ZombieHelper.Deaths++;
            PlayerPrefs.SetString("character-" + PlayerPrefs.GetInt("character"), JsonUtility.ToJson(Main.Instance.ZombieHelper));
        }

        /// <summary>
        /// Обновление сохраненных данных
        /// </summary>
        private void UpdateSavedData()
        {
            // Увеличиваем количество игр персонажа
            Main.Instance.ZombieHelper.Played++;

            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + Main.Instance.Coins);
            PlayerPrefs.SetInt("total-coins", PlayerPrefs.GetInt("total-coins") + Main.Instance.Coins);
            PlayerPrefs.SetInt("brains", PlayerPrefs.GetInt("brains") + Main.Instance.Brains);
        }

        /// <summary>
        /// Продолжение уровня после воскрешения персонажа
        /// </summary>
        public void ResumeLevel()
        {
            _losePanel.SetActive(false);
            _hintPanel.SetActive(false);

            // Запускаем уровень
            Main.Instance.LaunchALevel();
            // Восстанавливаем персонажа к последнему респауну
            Main.Instance.CharacterController.RestoreCharacter();
            // Возобновляем привязку камеры
            Main.Instance.SnapCameraToTarget();
        }
    }
}