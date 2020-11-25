using UnityEngine;

namespace Cubra
{
    public class ObjectChangesSpeed : MonoBehaviour
    {
        [Header("Новая скорость")]
        [SerializeField] private float _speed;

        private void OnTriggerStay2D(Collider2D collision)
        {
            GameManager.Instance.CharacterController.IsAccelerated = true;
            GameManager.Instance.CharacterController.SpeedUpCharacter(_speed);
        }
    }
}