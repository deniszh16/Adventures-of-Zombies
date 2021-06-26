using UnityEngine;

namespace Cubra
{
    public class Finish : CollisionObjects
    {
        [Header("Финишная кнопка")]
        [SerializeField] private GameObject _finishButton;
        
        public override void ActionsOnEnter(Character character)
        {
            if (character.Life)
            {
                if (GameManager.Instance.Brains == 0)
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
                if (GameManager.Instance.Brains == 0)
                {
                    _finishButton.SetActive(false);
                }
            }    
        }
    }
}