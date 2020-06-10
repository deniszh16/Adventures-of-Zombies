using UnityEngine;
using UnityEngine.UI;

namespace Cubra
{
    public class Brain : ObjectsToCollect
    {
        [Header("Количество мозгов")]
        [SerializeField] private Text _amountBrains;

        /// <summary>
        /// Действия при касании персонажа с коллайдером
        /// </summary>
        /// <param name="character">персонаж</param>
        public override void ActionsOnEnter(Character character)
        {
            character.SetSound(Character.Sounds.Brain);
            character.PlayingSound.PlaySound();

            // Уменьшаем количество мозгов
            Main.Instance.Brains--;
            UpdateQuantityBrains();

            InstanseObject.SetActive(false);
        }

        /// <summary>
        /// Обновление информации по количеству мозгов
        /// </summary>
        private void UpdateQuantityBrains()
        {
            if (Main.Instance.Brains > 0)
            {
                _amountBrains.text = "x" + Main.Instance.Brains;
            }
            else
            {
                _amountBrains.text = "ok";
                _amountBrains.color = new Color32(127, 255, 0, 255);
            }
        }
    }
}