using UnityEngine;

namespace Cubra
{
    public class ObjectChangesSpeed : MonoBehaviour
    {
        [Header("Новая скорость")]
        [SerializeField] private float _speed;

        private void OnTriggerStay2D(Collider2D collision)
        {
            Main.Instance.CharacterController.IsAccelerated = true;
            Main.Instance.CharacterController.SpeedUpCharacter(_speed);
        }
    }
}