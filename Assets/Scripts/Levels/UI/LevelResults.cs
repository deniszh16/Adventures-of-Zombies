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
            _starsHelper = JsonUtility.FromJson<StarsHelper>(PlayerPrefs.GetString("stars-level"));
            GameManager.Instance.CharacterController.IsDead += FailedLevel;
        }

        /// <summary>
        /// Завершение уровня после сбора всех мозгов
        /// </summary>
        public void CompleteLevel()
        {
            GameManager.Instance.CurrentMode = GameManager.GameModes.Completed;
            Invoke(nameof(WinningAtLevel), 0.7f);
        }

        /// <summary>
        /// Отображение результата после проигрыша
        /// </summary>
        public void FailedLevel()
        {
            GameManager.Instance.CurrentMode = GameManager.GameModes.Losing;
            Invoke(nameof(LosingAtLevel), 1.5f);
        }

        /// <summary>
        /// Победа на уровне
        /// </summary>
        public void WinningAtLevel()
        {
            UpdateSavedData();

            _victoryPanel.SetActive(true);
            _hintPanel.SetActive(true);

            if (PlayerPrefs.GetInt("progress") <= GameManager.Instance.LevelNumber)
                PlayerPrefs.SetInt("progress", GameManager.Instance.LevelNumber + 1);

            // Набранные на уровне очки
            var points = GameManager.Instance.Timer.Seconds * 55;
            _levelScore.text = points.ToString();
            PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + points);

            _textHint.ChangeKey(_winningTexts[GameManager.Instance.Stars - 1]);

            PlayerPrefs.SetString("character-" + PlayerPrefs.GetInt("character"), JsonUtility.ToJson(GameManager.Instance.ZombieHelper));

            _levelStars.sprite = _spritesStars[GameManager.Instance.Stars - 1];
            SaveStars();
        }

        /// <summary>
        /// Запись результата по звездам
        /// </summary>
        private void SaveStars()
        {
            if (_starsHelper.Stars.Count < GameManager.Instance.LevelNumber)
            {
                _starsHelper.Stars.Add(GameManager.Instance.Stars);
                PlayerPrefs.SetString("stars-level", JsonUtility.ToJson(_starsHelper));
            }
            else if (_starsHelper.Stars[GameManager.Instance.LevelNumber - 1] < GameManager.Instance.Stars)
            {
                _starsHelper.Stars[GameManager.Instance.LevelNumber - 1] = GameManager.Instance.Stars;
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

            if (PlayerPrefs.GetInt("coins") < 50 || GameManager.Instance.Timer.Seconds <= 0)
                _resurrect.interactable = false;

            _textHint.ChangeKey("result-lose");

            GameManager.Instance.ZombieHelper.Deaths++;
            PlayerPrefs.SetString("character-" + PlayerPrefs.GetInt("character"), JsonUtility.ToJson(GameManager.Instance.ZombieHelper));
        }

        /// <summary>
        /// Обновление сохраненных данных
        /// </summary>
        private void UpdateSavedData()
        {
            GameManager.Instance.ZombieHelper.Played++;

            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + GameManager.Instance.Coins);
            PlayerPrefs.SetInt("total-coins", PlayerPrefs.GetInt("total-coins") + GameManager.Instance.Coins);
            PlayerPrefs.SetInt("brains", PlayerPrefs.GetInt("brains") + GameManager.Instance.Brains);
        }

        /// <summary>
        /// Продолжение уровня после воскрешения персонажа
        /// </summary>
        public void ResumeLevel()
        {
            _losePanel.SetActive(false);
            _hintPanel.SetActive(false);

            GameManager.Instance.LaunchALevel();
            GameManager.Instance.CharacterController.RestoreCharacter();
            GameManager.Instance.SnapCameraToTarget();
        }
    }
}