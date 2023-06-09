using Logic.Characters;
using Logic.UsefulObjects;
using UnityEngine;

namespace Logic.Levels
{
    public class LevelFinish : MonoBehaviour
    {
        [Header("Финишная кнопка")]
        [SerializeField] private GameObject _finishButton;
        
        [Header("Компонент мозгов")]
        [SerializeField] private BrainsAtLevel _brainsAtLevel;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (_brainsAtLevel.Brains == 0)
            {
                if (col.TryGetComponent(out Character character))
                {
                    if (character.Life)
                        _finishButton.SetActive(true);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponent<Character>())
                _finishButton.SetActive(false);
        }
    }
}