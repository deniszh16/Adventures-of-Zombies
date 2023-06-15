using Logic.Characters;
using UnityEngine;

namespace Logic.UsefulObjects
{
    public class Bone : MonoBehaviour
    {
        [Header("Компонент костей")]
        [SerializeField] private BonesAtLevel _bonesAtLevel;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out CharacterSounds characterSounds))
            {
                characterSounds.SetSound(Sounds.Brain);
                characterSounds.PlaySound();
                
                _bonesAtLevel.AddBones();
                gameObject.SetActive(false);
            }
        }
    }
}