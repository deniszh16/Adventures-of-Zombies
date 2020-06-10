using UnityEngine;

namespace Cubra
{
    public class Respawn : CollisionObjects
    {
        /// <summary>
        /// Действия при касании персонажа с коллайдером
        /// </summary>
        /// <param name="character">персонаж</param>
        public override void ActionsOnEnter(Character character)
        {
            if (character.Life)
            {
                Main.Instance.CharacterController.RespawnPosition = transform.position + Vector3.up;
            }
        }
    }
}