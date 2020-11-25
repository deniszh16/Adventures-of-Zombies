using System;
using Cinemachine;
using UnityEngine;
using Cubra.Helpers;
using Cubra.Controllers;

namespace Cubra
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        // Перечисление режимов игры
        public enum GameModes { Start, Training, Play, Completed, Losing }

        public GameModes CurrentMode { get; set; }

        [Header("Номер уровня")]
        [SerializeField] private int _levelNumber;

        public int LevelNumber => _levelNumber;

        // Событие по запуску уровня
        public event Action LevelLaunched;

        // Ссылка на обучение игрока
        private Training _training;

        [Header("Префабы персонажей")]
        [SerializeField] private GameObject[] characters;

        [Header("Стартовая позиция")]
        [SerializeField] private Vector3 position;

        private Character _character;
        public Controllers.CharacterController CharacterController { get; private set; }

        // Объект для работы со статистикой по персонажам
        public ZombieHelper ZombieHelper { get; set; }

        [Header("Количество мозгов")]
        [SerializeField] private int _brains;

        public int Brains { get => _brains; set => _brains = value; }

        // Количество собранных монет
        public int Coins { get; set; }

        // Текущее количество звезд за уровень
        public int Stars { get; set; } = 3;

        [Header("Виртуальная камера")]
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtual;

        [Header("Таймер уровня")]
        [SerializeField] private Timer _timer;

        public Timer Timer => _timer;

        [Header("Отсчет для старта")]
        [SerializeField] private CountdownToStart _countdown;

        public CountdownToStart Countdown => _countdown;

        // Ссылка на компонент результатов
        public LevelResults LevelResults { get; private set; }

        // Ссылка на компонент фоновой музыки
        private BackgroundMusic _backgroundMusic;

        private void Awake()
        {
            Instance = this;

            // Номер активного персонажа
            var activeCharacter = PlayerPrefs.GetInt("character");
            // Создаем активного персонажа в стартовой позиции и получаем его компонент
            _character = Instantiate(characters[activeCharacter - 1], position, Quaternion.identity).GetComponent<Character>();

            SnapCameraToTarget();

            // Преобразовваем сохраненную json строку в объект
            ZombieHelper = JsonUtility.FromJson<ZombieHelper>(PlayerPrefs.GetString("character-" + PlayerPrefs.GetInt("character")));

            CharacterController = gameObject.GetComponent<Controllers.CharacterController>();

            LevelResults = gameObject.GetComponent<LevelResults>();
            _training = gameObject.GetComponent<Training>();
            _backgroundMusic = FindObjectOfType<BackgroundMusic>();
        }

        private void Start()
        {
            // Если на уровне присутствует обучение
            if (_training & Training.PlayerTraining)
            {
                CurrentMode = GameModes.Training;
                CharacterController.Disable();

                _ = StartCoroutine(_training.StartTraining());
            }
            else
            {
                Invoke("LaunchALevel", 0.2f);
            }
        }
        
        public void LaunchALevel()
        {
            CurrentMode = GameModes.Play;
            LevelLaunched?.Invoke();

            // Если звуки не отключены
            if (SoundController.PlayingSounds)
            {
                // Устанавливаем фоновую музыку и запускаем
                _backgroundMusic.SetBackgroundMusic();
                _backgroundMusic.SwitchMusic((int)BackgroundMusic.State.On);
            }    
        }

        /// <summary>
        /// Привязка/отвязка камеры от персонажа
        /// </summary>
        public void SnapCameraToTarget()
        {
            _cinemachineVirtual.Follow = _character.Life ? _character.Transform : null;
        }
    }
}