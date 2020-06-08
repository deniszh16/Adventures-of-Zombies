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

        // Ссылки на компоненты
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
                // Скрываем части персонажа
                _parts.SetActive(false);
                // Отображаем полного персонажа
                _zombie.SetActive(true);

                // Обновляем статистику по персонажу
                _zombieStats.UpdateStatistics();
                // Обновляем текст на активной кнопке
                _zombieSelection.ChangeButton(true);

                // Подписываем обновление кнопок в событие смены персонажа
                _buttonUpdateHelper.ZombieSelected += _zombieSelection.ChangeButton;
            }
            else
            {
                // Обновляем текст на отключенной кнопке
                _zombieSelection.ChangeButton(false);
            }
        }

        /// <summary>
        /// Выбор текущего персонажа
        /// </summary>
        public void ChooseZombie()
        {
            // Сохраняем номер выбранного персонажа
            PlayerPrefs.SetInt("character", _number);
            // Обновляем кнопки выбора у персонажей
            _buttonUpdateHelper.UpdateButtons();
        }
    }
}