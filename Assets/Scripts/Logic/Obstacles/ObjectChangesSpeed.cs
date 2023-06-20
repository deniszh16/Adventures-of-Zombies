using Logic.Characters;
using UnityEngine;

namespace Logic.Obstacles
{
    public class ObjectChangesSpeed : MonoBehaviour
    {
        [Header("Новая скорость")]
        [SerializeField] private float _speed;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.TryGetComponent(out CharacterControl characterControl))
                characterControl.SpeedUpCharacter(_speed);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out CharacterControl characterControl))
                characterControl.IsAccelerated = false;
        }
    }
}