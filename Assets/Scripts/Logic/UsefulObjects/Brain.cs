using Logic.Characters;
using UnityEngine;

namespace Logic.UsefulObjects
{
    public class Brain : MonoBehaviour
    {
        [Header("Компонент мозгов")]
        [SerializeField] private BrainsAtLevel brainsAtLevel;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out CharacterSounds characterSounds))
            {
                characterSounds.SetSound(Sounds.Brain);
                characterSounds.PlaySound();
                
                brainsAtLevel.SubtractBrains(1);
                gameObject.SetActive(false);
            }
        }
    }
}