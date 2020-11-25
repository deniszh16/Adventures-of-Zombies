using UnityEngine;
using Cubra.Helpers;

namespace Cubra
{
    public class Zombie : MonoBehaviour
    {
        [Header("Номер персонажа")]
        [SerializeField] private int _number;

        public int Number => _number;

        [Header("Прогресс для открытия")]
        [SerializeField] private int _progress;

        [Header("Части персонажа")]
        [SerializeField] private GameObject _parts;

        [Header("Полный персонаж")]
        [SerializeField] private GameObject _zombie;
        
        private ZombieStats _zombieStats;
        private ZombieSelection _zombieSelection;
        private ButtonUpdateHelper _buttonUpdateHelper;

        private void Awake()
        {
            _zombieStats = gameObject.GetComponent<ZombieStats>();
            _zombieSelection = gameObject.GetComponent<ZombieSelection>();
            _buttonUpdateHelper = gameObject.GetComponentInParent<ButtonUpdateHelper>();
        }

        private void Start()
        {
            // Если прогресс достаточный для открытия
            if (_progress <= PlayerPrefs.GetInt("progress"))
            {
                _parts.SetActive(false);
                _zombie.SetActive(true);

                _zombieStats.UpdateStatistics();
                _zombieSelection.ChangeButton(true);

                // Подписываем обновление кнопок в событие смены персонажа
                _buttonUpdateHelper.ZombieSelected += _zombieSelection.ChangeButton;
            }
            else
            {
                _zombieSelection.ChangeButton(false);
            }
        }

        /// <summary>
        /// Выбор текущего персонажа
        /// </summary>
        public void ChooseZombie()
        {
            PlayerPrefs.SetInt("character", _number);
            _buttonUpdateHelper.UpdateButtons();
        }
    }
}