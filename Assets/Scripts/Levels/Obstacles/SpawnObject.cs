using System.Collections;
using UnityEngine;

namespace Cubra
{
    public class SpawnObject : MonoBehaviour
    {
        [Header("Время до создания")]
        [SerializeField] private float _pause;

        [Header("Пул объектов")]
        [SerializeField] private GameObject _pool;

        // Номер доступного объекта в пуле
        private int _objectNumber;

        // Ссылка на музыкальный компонент
        private PlayingSound _playingSound;

        private void Awake()
        {
            _playingSound = GetComponent<PlayingSound>();
        }

        private void Start()
        {
            Main.Instance.LevelLaunched += StartCreation;
        }

        /// <summary>
        /// Запуск создания объектов
        /// </summary>
        private void StartCreation()
        {
            _ = StartCoroutine(CreateObject());
        }

        /// <summary>
        /// Переодическое создание (взятие из пула) объектов
        /// </summary>
        private IEnumerator CreateObject()
        {
            while (Main.Instance.CurrentMode == Main.GameModes.Play)
            {
                yield return new WaitForSeconds(_pause);

                // Если в пуле есть объекты
                if (_pool.transform.childCount > 0)
                {
                    var obj = _pool.transform.GetChild(_objectNumber);

                    obj.position = transform.position;
                    obj.rotation = transform.rotation;

                    obj.gameObject.SetActive(true);
                    _playingSound.PlaySound();

                    _objectNumber++;

                    if (_objectNumber >= _pool.transform.childCount)
                    {
                        _objectNumber = 0;
                    }
                }
            }
        }
    }
}