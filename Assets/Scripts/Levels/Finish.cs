using UnityEngine;

namespace Cubra
{
    public class Finish : CollisionObjects
    {
        [Header("Финишная кнопка")]
        [SerializeField] private GameObject _finishButton;

        /// <summary>
        /// Действия при касании персонажа с коллайдером
        /// </summary>
        /// <param name="character">персонаж</param>
        public override void ActionsOnEnter(Character character)
        {
            if (character.Life)
            {
                // Если собраны все мозги
                if (Main.Instance.Brains == 0)
                {
                    _finishButton.SetActive(true);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var character = collision.GetComponent<Character>();

            if (character)
            {
                if (Main.Instance.Brains == 0)
                {
                    _finishButton.SetActive(false);
                }
            }    
        }
    }
}