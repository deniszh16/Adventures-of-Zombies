using UnityEngine;

namespace Cubra
{
    public class ObjectChangesSpeed : MonoBehaviour
    {
        [Header("Новая скорость")]
        [SerializeField] private float _speed;

        private void OnTriggerStay2D(Collider2D collision)
        {
            // Активируем ускорение персонажа
            Main.Instance.CharacterController.IsAccelerated = true;
            // Обновляем скорость персонажа
            Main.Instance.CharacterController.SpeedUpCharacter(_speed);
        }
    }
}