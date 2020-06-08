using UnityEngine;
using UnityEngine.UI;
using Cubra.Helpers;

namespace Cubra
{
    public class ZombieStats : MonoBehaviour
    {
        [Header("Количество игр")]
        [SerializeField] private Text _played;

        [Header("Количество поражений")]
        [SerializeField] private Text _loss;

        [Header("Идентификатор достижения")]
        [SerializeField] private string _identifier;

        // Объект для работы со статистикой по персонажу
        public ZombieHelper ZombieHelper { get; private set; } = new ZombieHelper();

        private void Awake()
        {
            var number = gameObject.GetComponent<Zombie>().Number;
            ZombieHelper = JsonUtility.FromJson<ZombieHelper>(PlayerPrefs.GetString("character-" + number));
        }

        /// <summary>
        /// Обновление статистики по персонажу
        /// </summary>
        public void UpdateStatistics()
        {
            _played.gameObject.SetActive(true);
            _played.GetComponent<TextTranslation>().TranslateText();
            // Выводим количество сыгранных игр
            _played.text += " " + ZombieHelper.Played;

            if (ZombieHelper.Played >= 15)
            {
                if (Application.internetReachability != NetworkReachability.NotReachable)
                    // Открываем достижение по количеству игр за персонажа
                    GooglePlayServices.UnlockingAchievement(_identifier);
            }

            _loss.gameObject.SetActive(true);
            _loss.GetComponent<TextTranslation>().TranslateText();
            // Выводим количество смертей
            _loss.text += " " + ZombieHelper.Deaths;
        }
    }
}