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
            // Устанавливаем звук и воспроизводим
            character.SetSound(Character.Sounds.Brain);
            character.PlayingSound.PlaySound();

            // Уменьшаем количество оставшихся мозгов
            Main.Instance.Brains--;
            // Обновляем информацию по мозгам
            UpdateQuantityBrains();

            // Отключаем объект
            InstanseObject.SetActive(false);
        }

        /// <summary>
        /// Обновление информации по количеству мозгов
        /// </summary>
        private void UpdateQuantityBrains()
        {
            if (Main.Instance.Brains > 0)
            {
                // Выводим число оставшихся мозгов на уровне
                _amountBrains.text = "x" + Main.Instance.Brains;
            }
            else
            {
                _amountBrains.text = "ok";
                // Перекрашиваем текст в зеленй цвет
                _amountBrains.color = new Color32(127, 255, 0, 255);
            }
        }
    }
}